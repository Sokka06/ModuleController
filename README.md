# ModuleController
Something useful for character controllers, view controllers, audio controllers.

![modulecontroller](https://user-images.githubusercontent.com/54811990/142493660-2f6a2e08-5cec-4284-9462-8664e301eb6c.png)

## Getting started
1. Clone or download repository.
2. Copy ModuleController from Plugins folder to your project.
3. Take a look at the included Demos to see examples how you can introduce a bit more modularity into your Character Controllers and such.
4. Done!

## How To Use
1. Create new scripts for your *Module Controller* (named *FooModuleController*, for example) and for your *Module base class* (named *AbstractFooModule*, for example).
2. If your scripts are *MonoBehaviours*, you can inherit from **ModuleControllerBehaviour** and **ModuleBehaviour**, which are also *MonoBehaviours* and include all the needed boilerplate code. Alternatively you can implement **IModuleController** and **IModule** interfaces to customize your scripts more.
3. Make new scripts for your *Modules* (named *FooBarModule, FooBazModule*, etc) and make them inherit from your *module base class*. You can override your module's *SetupModule* method and add `Debug.Log($"Hello {GetType().Name}");` to make sure they're setup correctly.
4. Make a new *GameObject* and add your *Module Controller* and *Modules* on it.
5. Press play!
