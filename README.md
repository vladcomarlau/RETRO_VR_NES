# Retro VR NES Emulator

The project is an immersive virtual reality experience that allows users to play classic NES (Nintendo Entertainment System) titles in a simulated 3D space. 

The player is placed in an interactive environment (a retro room with a CRT TV or an arcade room) and has the freedom to physically interact with the virtual equipment.

## Core Features

- **Game Selection and Running:** Interactive menu for loading NES games. The emulator's video output is mapped in real-time onto the screens of the 3D objects in the Unity scene.
- **VR Locomotion:** Teleportation-based movement system that allows space exploration and optimal positioning of the user.
- **Environment Interaction:** Using VR controllers to physically press the console buttons, the screens' control elements, and the menu interface.

## Technologies Used

- **Hardware:** Meta Quest 2
- **Game Engine:** [Unity](https://unity.com/)
- **VR Framework:** Meta XR Core / OpenXR
- **Emulation System:** [UNES](https://github.com/ls9512/UNES) (Unity NES Emulator)

## Team
- RUCSĂNDOIU Andrei
- COMÂRLĂU Vlad-Constantin

## Setup and Running

1. **Requirements:**
   * Unity Hub with a compatible Unity version (LTS recommended).
   * *Android Build Support* module and *Meta XR SDK* installed.
   * Meta Quest 2 headset configured in *Developer* mode.

2. **Installation:**
   ```bash
   git clone https://github.com/username/repo-name.git
   ```

3. **Running:**
   * Open the project in Unity.
   * Ensure the Meta XR Core SDK and UNES packages are imported correctly.
   * Connect the Meta Quest 2 headset and compile the application using *Build and Run*.

## License

The project uses [UNES](https://github.com/ls9512/UNES) for emulation.
