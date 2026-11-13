# Retro VR NES Emulator

Proiectul este o experiență imersivă în realitate virtuală care permite utilizatorilor să joace titluri clasice NES (Nintendo Entertainment System) într-un spațiu 3D simulat. 

Aplicația pune accent pe emularea fidelă a jocurilor retro și pe crearea unui sentiment de prezență. Jucătorul este plasat într-un mediu interactiv (o încăpere retro cu televizor CRT sau o sală arcade) și are libertatea de a interacționa fizic cu echipamentele virtuale.

## Funcționalități Principale

* **Selecția și rularea jocurilor:** Meniu interactiv pentru încărcarea jocurilor NES. Output-ul video al emulatorului este mapat în timp real pe ecranele obiectelor 3D din scena Unity.
* **Locomoție VR:** Sistem de deplasare prin teleportare care permite explorarea spațiului și poziționarea optimă a utilizatorului.
* **Interacțiune cu mediul:** Utilizarea controllerelor VR pentru a apăsa fizic butoanele consolei, elementele de control ale ecranelor și interfața meniului.

## Tehnologii Folosite

* **Hardware:** Meta Quest 2
* **Game Engine:** [Unity](https://unity.com/)
* **VR Framework:** Meta XR Core / OpenXR
* **Sistem de Emulare:** [UNES](https://github.com/ls9512/UNES) (Unity NES Emulator)

## Echipa
* RUCSĂNDOIU Andrei
* COMÂRLĂU Vlad-Constantin

## Configurare și Rulare

1. **Cerințe:**
   * Unity Hub cu o versiune Unity compatibilă (LTS recomandat).
   * Modulul *Android Build Support* instalat.
   * Casca Meta Quest 2 configurată în modul *Developer*.

2. **Instalare:**
   ```bash
   git clone https://github.com/username/nume-repo.git
   ```

3. **Rulare:**
   * Deschideți proiectul în Unity.
   * Verificați ca pachetele Meta XR Core SDK și UNES să fie importate corect.
   * Conectați casca Meta Quest 2 și compilați aplicația folosind *Build and Run*.

## Licență

Proiectul utilizează [UNES](https://github.com/ls9512/UNES) pentru emulare. 
