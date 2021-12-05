using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Tests;
using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.TestTools;

public class ModuleControllerTests
{
    /// <summary>
    /// Makes sure Modules are found.
    /// </summary>
    [Test]
    public void _01ModulesAreFound()
    {
        var controller = InstantiateModuleController();

        var modules = controller.FindModules();
        
        Assert.IsNotEmpty(modules);

        Debug.Log($"Found {modules.Length} modules:");
        foreach (var module in modules)
        {
            Debug.Log(module.GetType().Name);
        }
    }
    
    /// <summary>
    /// Makes sure Modules are found.
    /// </summary>
    [Test]
    public void _02ModulesAreInOrder()
    {
        var controller = InstantiateModuleController();
        
        // First module should be a Setup Module.
        Assert.IsInstanceOf<TestSetupModule>(controller.Modules[0]);
        
        // Second module should be a Name Module with a name of Alpha.
        Assert.IsInstanceOf<TestNameModule>(controller.Modules[1]);
        Assert.True(string.Equals("Alpha", (controller.Modules[1] as TestNameModule).Name));
        
        // Last Module should be an Update Module.
        Assert.IsInstanceOf<TestUpdateModule>(controller.Modules[controller.Modules.Count - 1]);
    }
    
    /// <summary>
    /// Makes sure SetupModule is ran on all Modules.
    /// </summary>
    [Test]
    public void _03SetupIsRan()
    {
        var controller = InstantiateModuleController();
        
        // Modules list should be initialized.
        Assert.NotNull(controller.Modules);
        
        for (int i = 0; i < controller.Modules.Count; i++)
        {
            // Controller should be set by SetupModule.
            Assert.NotNull(controller.Modules[i].Controller);
        }
    }
    
    /// <summary>
    /// Makes sure first Module of given type is returned.
    /// </summary>
    [Test]
    public void _04ReturnsModuleForType()
    {
        var controller = InstantiateModuleController();

        var nameModule = controller.GetModule<TestNameModule>();
        
        Assert.NotNull(nameModule);
        Assert.IsInstanceOf<TestNameModule>(nameModule);
        Assert.True(string.Equals("Alpha", nameModule.Name));
        
        Debug.Log($"Test Module: {nameModule.Name}");
    }
    
    /// <summary>
    /// Makes sure all modules for given type are found.
    /// </summary>
    [Test]
    public void _05ReturnsModulesForType()
    {
        var controller = InstantiateModuleController();

        // Limit Name modules to first 3.
        var modules = new TestNameModule[3];
        var count = controller.GetModules(ref modules);
        
        Assert.Greater(count, 1);

        for (int i = 0; i < count; i++)
        {
            var module = modules[i];
            Debug.Log($"Test Module: {module.Name}");

            Assert.NotNull(module);
            Assert.IsInstanceOf<TestNameModule>(module);
        }
    }

    /// <summary>
    /// Makes sure modules are updated properly.
    /// </summary>
    /// <returns></returns>
    [UnityTest]
    public IEnumerator _06UpdateIsRan()
    {
        var controller = InstantiateModuleController();

        // Run FixedUpdate for 5 seconds.
        const float waitTime = 5f;
        yield return WaitForFixedUpdates(waitTime);

        var updateModule = controller.GetModule<TestUpdateModule>();
        Assert.GreaterOrEqual(updateModule.RunTime, waitTime);
        
        Debug.Log($"Update Module Run Time: {updateModule.RunTime}s");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    /*[UnityTest]
    public IEnumerator ReturnsGivenTypeWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }*/

    /// <summary>
    /// Instantiates a module controller for testing from a prefab.
    /// </summary>
    /// <returns></returns>
    private TestModuleController InstantiateModuleController()
    {
        var prefab = AssetDatabase.LoadAssetAtPath("Assets/Tests/PlayMode/TestModuleController.prefab", typeof(TestModuleController)) as TestModuleController;
        return GameObject.Instantiate(prefab);
    }

    private IEnumerator WaitForFixedUpdates(float waitTime)
    {
        var timer = waitTime;
        while (timer > 0f)
        {
            timer -= Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
}
