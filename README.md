# Vertigo Games Wheel Spin Demo

This repository contains the Wheel Spin demo project assigned by Vertigo Games. The release package includes an APK of the demo as well as a video from the first couple of levels of the game.

## Requirements

- **Unity 2021.3.45f2.** Other 2021.3 LTS versions should also work.
- **Android Build Support** module, only if you want to build the APK yourself.

No extra setup is needed. DOTween is included under `Assets/Plugins`. TextMeshPro, uGUI and the Test Framework are installed by the Package Manager when the project is first opened.

## Getting Started

1. Clone the repository:
   ```
   git clone https://github.com/poyrazogluyigit/vertigo-demo.git
   ```
2. In Unity Hub, choose **Add → Add project from disk**, select the cloned folder and open it with 2021.3.45f2. The first import takes a few minutes.
3. Open `Assets/Scenes/WheelSpin.unity`.
4. In the Game view, pick a landscape resolution. The UI was checked at 1920×1080 (16:9), 1440×1080 (4:3) and 2400×1080 (20:9).
5. Press **Play**.

## Running the Tests

Open **Window → General → Test Runner**, select the **EditMode** tab and press **Run All**.

The tests are in `Assets/Tests`. They cover the game flow state machine, reward bookkeeping, the event bus, the zone rules and the amount labels. Fakes stand in for the views and the reward service, so no scene is needed.

## Building for Android

1. Open **File → Build Settings**, select **Android** and press **Switch Platform**.
2. Press **Build**. `WheelSpin.unity` is the only scene in the build.

The game is landscape only, with a minimum SDK of 22. A prebuilt APK is attached to the latest GitHub release.

## Editing the Wheels

All wheel content is edited from the Inspector. No code changes are needed.

| Asset | What it controls |
|---|---|
| `Assets/WheelSpin/Wheels/` | One `WheelContent` per wheel: 8 slices, each a `RewardDefinition`. `Bronze_xx` are normal wheels with exactly one bomb. `Silver_xx` (safe) and `Gold_xx` (super) have no bomb. |
| `Assets/WheelSpin/Default Wheel Picker` | Which wheels each zone type picks from. A wheel is picked at random from its list on every level. The picker reports an error in the Console for any wheel with the wrong number of bombs for its zone. |
| `Assets/WheelSpin/Reward Definitions/` | Each reward's icon, base amount and scaling curve. |
| `Assets/WheelSpin/Scaling/`, `Reward Scaler` | How reward amounts grow with the level. |

The zone rhythm itself (every 5th zone is safe, every 30th is super, exit is allowed only on those zones) lives in `Assets/Scripts/Core/Zones.cs`.

## Project Structure

```
Assets/Scripts/
├── Core/          Game rules and flow: GameFlow (state machine), RewardService, EventBus, Zones, events, interfaces
├── Data/          ScriptableObjects: WheelContent, WheelPicker, RewardDefinition, RewardScaler, EndScreen
├── Views/         MonoBehaviours that draw what the events tell them; InputManager turns button clicks into events
└── GameManager    Composition root: builds the services and hands every view the event bus
```

`GameFlow` never references a view. It publishes events such as `RewardsReady`, `SpinStarted` and `BombHit`, and it waits for the wheel to report `SpinFinished` before moving on. Views hold no game state. They only display the data carried by the events.

## Screenshots

### 16:9
![16:9 Aspect Ratio](images/16-9.png)
### 4:3
![4:3 Aspect Ratio](images/4-3.png)
### 20:9
![20:9 Aspect Ratio](images/20-9.png)

## Revision Notes

Changes made in response to the review of the first submission:

**1. The exit rule was not enforced.**
I had misread the brief and built it like the card game in Critical Strike. This is now fixed: the EXIT button is only active on safe zones (every 5th) and super zones (every 30th). The rule is defined in one place, `Zones.AllowsExit`. `GameFlow` also rejects any exit request that breaks the rule, whatever state the button is in. The rule is covered by `ZonesTests`.

**2. There was no state management in the game flow.**
The game flow now runs on an explicit state machine (`GameFlow`: `StartingLevel → Idle → Spinning → CashedOut / GameOver`). Each input is only accepted in the state where it is valid: spin only in `Idle`, restart only after the game has ended. Views hold no game state; they only display the data the events give them. As a result, the end screen is redrawn from the current game's rewards every time, and rewards from a previous game can no longer appear. `GameFlowTests` checks that the buttons stay disabled after a bomb, that restarting clears the rewards, and that pressing spin while the wheel is spinning is ignored.

**3. The "exactly one bomb" rule, zone 1, and reward variety.**
I rebuilt the wheel setup. `WheelContent` (formerly `WheelSO`) is a data container that only holds a wheel's eight slices. `WheelPicker` decides which wheel is shown on which level. Since only the picker knows which zone type a wheel is used for, the bomb count is checked in the picker's `OnValidate`, against each wheel's zone type. A wheel that breaks the rule is reported as an error in the Console.
The `level == 1` exception has been removed, so zone 1 now has a bomb like any other normal zone.
Each zone type now has several wheels (10 bronze, 5 silver, 5 gold), and one is picked at random on every level, so consecutive zones usually offer different sets of rewards. Amounts grow with the level, each reward following its own scaling curve.

**4. Architecture and code standards.**
- **Event bus and coupling:** An event bus was implemented, so the parts of the game are no longer tightly coupled to each other. The static events have been removed.
- **Interfaces and DI:** Services are used through interfaces (`IEventBus`, `IRewardService`, `IWheelSchedule`, `IRewardScaler`, `IRandom`). Dependencies are passed in through constructors, and `GameManager` builds them all as the composition root. This is what lets `GameFlow` and `RewardService` run in tests with fake dependencies.
- **Naming and constants:** The code is in the `WheelSpin` namespace and split into two assemblies (`Vertigo` and `Tests`). Private fields use `_camelCase`. The numbers behind the game rules are now named constants, such as the zone intervals in `Zones` and the slice count in `WheelContent.SliceCount`.
- **async void and tweens:** The `async void` flow has been removed. `GameFlow` is synchronous and waits for the wheel's `SpinFinished` event, so there is no async flow left where an exception could go unobserved. Tweens are linked to their owning object with `SetLink(gameObject)`, so they are killed automatically when it is destroyed.

**5. Gameplay and visual polish.**
- **Effects:**
  - *Spin:* The wheel winds up, spins, slightly overshoots its target and settles back. The indicator ticks against every slice, and the rays behind the wheel pulse harder as the spin nears its end.
  - *Win:* The winning slice glows and pops. Its row in the reward list bounces and the amount counts up.
  - *Bomb:* The bomb blinks red at a quickening pace and explodes, followed by a screen shake and a red flash. The end screen only opens once the effect has finished.
- **Icon sizes:** The empty space around the icons was cropped. Every icon is sized to cover the same visual area whatever its shape, so a square badge and a 3:1 weapon look equally heavy.
- **Text sizes:** All text uses one font (Lato Black) on a four-step scale: 56 / 44 / 36 / 28. Auto Size is off.
- **UI overlaps:** I could not find any place where UI elements visually overlap. Some of the transparent parts may do so (like in the wheel), but I failed to find an overlap in any of the reward lists. Still, they are modified to make sure no elements overlap.

**Delivery.** The README now covers setup, tests, building and content editing.
