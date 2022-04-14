# RPG-Battle-Clone-v2
Create a battle mechanic for a RPG game
## Project
- Unity 2020.3.6.1
- Android Platform
## 3rd Party
- TextMeshPro
## Overview
### Structure
There is four main folders :
- **Framework :** a basic service-system-manager design to handle initialization process.
- **Game :** three main namespaces; Common, HeroSelection, Battle.
- **UI System :** a simple UI solution. It manages menus and popups.
- **Save Utility :** All the savable data used in this project is based save utility.
### Design
#### Framework
Framework handles service-system-manager design. Service has its systems. Every system has its managers. Managers are independent.
To locate a system you should use system's service locator. To locate a manager you should use system's internal manager locator.
#### UI System
Independent system for handling UI responsibilities. Menu and popup dependencies should be given with a settings object.
#### Save Utility
In project, data is held by PlayerPrefs with Json. ScriptableObjects are used as data views for controllers. SavableScriptableObject is Save Utility's feature.
#### Gameplay
- Multiple scene design is used. Main Scene contains Level System. Level System acts like state handler and scene manager. This features could be split into different classes.
- Every scene has Game Manager. Game Manager evaluates state of the game and starts gameplay.
- Every gameplay scene contains gameplay specific systems managers and controllers.
#### Signals
Project uses generic PubSub pattern for events. Every signal can be instantiated through Reflection then can be cached.
#### Commands
Even I wanted to use Task based operations limitations of Unity forced me to use Command Pattern in various scenarios.
Feedbacks and attack actions are Commands.

