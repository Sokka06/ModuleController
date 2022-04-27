# ModuleController
![modulecontroller](https://user-images.githubusercontent.com/54811990/142493660-2f6a2e08-5cec-4284-9462-8664e301eb6c.png)

## What is it?
Module Controller helps you separate your Character Controllers, Vehicle Controllers, and others into smaller modular pieces (aka *Modules*). This helps you keep your scripts more focused and makes it easier to modify and add new features.

## Getting started
1. Clone or download the repository.
2. Copy **ModuleController** folder from **Plugins** folder to your project.
3. Take a look at the included *Demos* to see examples of how you can introduce a bit more modularity into your *Character Controllers* and such.
4. Done!

Developed in **Unity 2020.3.23f1**, but I don't see why it wouldn't work on any Unity version.

## How To Use
1. Create new scripts for your *Module Controller* (named *FooModuleController*, for example) and your *Module base class* (named *AbstractFooModule*, for example).
2. If your classes are *MonoBehaviours*, you can inherit from **ModuleControllerBehaviour** and **ModuleBehaviour**, which are also *MonoBehaviours* and include all the needed boilerplate code. Alternatively, you can implement **IModuleController** and **IModule** interfaces to customize them further.
3. In your *Module Controller* script, add `SetupModules();` to your **Start** function and `UpdateModules(Time.deltaTime);` to your **Update** or **FixedUpdate** function.
4. Make new scripts for your *Modules* (named *FooBarModule, FooBazModule*, etc) and make them inherit from your *Module base class*. You can override your module's *SetupModule* method and add `Debug.Log($"Hello {GetType().Name}");` to confirm that their Setup is called.
5. Make a new *GameObject* and add your new *Module Controller* and *Module* scripts to it.
6. Press play!

## Demos
Basic Character Controller that uses separate Modules for 
3rd Person Character with more advanced Module Controllers.
Vehicle Controller.

## Notes
- *Modules* can also be *Module Controllers* and have their own submodules. Just implement **IModuleController** in your *Module*, add all the necessary boilerplate code and make scripts for its *Modules*.
- Adding and removing *Modules* during *runtime* is currently not implemented.
