# One Frame Events

There is simple event system for Unity DOTS.
It uses .NET SourceGenerator to generate systems and jobs for control one-frame events.

## Importing

To use this in your Unity project import it from Unity Package Manager. You can [download it and import it from your hard drive](https://docs.unity3d.com/Manual/upm-ui-local.html), or [link to it from github directly](https://docs.unity3d.com/Manual/upm-ui-giturl.html).

## Usage

Implement an event component and inherit it from IOneFrameEvent.

```csharp
using ED.DOTS.OneFrameEvents;
using Unity.Entities;

namespace ExampleNamespace
{
    public struct ExampleEvent : IComponentData, IOneFrameEvent
    {
        public int ExampleValue;
    }
}
```

The resource generator will generate a request and a system for it.

```csharp
struct ExampleEvent_Request : IComponentData { ... }
struct ExampleEvent_System : ISystem { ... }
```

Once the entity with the request is created, the system will process it and rise an event for one next frame.

```csharp
// EntityCommandBuffer ecb;
ecb.AddComponent(ecb.CreateEntity(), new ExampleEvent_Request
{
    Value = new ExampleEvent { ExampleValue = 123 }
});
```

```csharp
public partial struct OnExampleEventJob : IJobEntity
{
    void Execute(in ExampleEvent exampleEvent)
    {
        Debug.Log(string.Format("ExampleEvent! value: {0}", exampleEvent.ExampleValue));
    }
}
```

All generated systems will be updated in the group `OneFrameEventSimulationSystemGroup`.

You can check the generator logs at the following path (Windows): `C:/Users/<username>/AppData/Local/Temp/Unity/SourceGenerator/OneFrameEvents/Log.txt`