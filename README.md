# jUIce
jUIce is a MVVM UI framework built on top of [Unity's uGui](https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/index.html) solution. It provides a series of systems and guidelines to boost your runtime UI development within Unity.

It aims to split the UI workflow into two distinct phases: technical and stylistic. This means that programmers and designers can cooperate together to achieve the final version of the UI. What this also means is that jUIce requires some technical insight to be used; you'll need to code your ViewModels.

This project is inspired by the amazing [deVoid UI Framework](https://github.com/yankooliveira/uiframework) by Yanko Oliveira. 

## Framework's Philosophy
The framework encourages a [Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) approach, splitting the main concerns of any UI piece into a couple of classes that interoperate together.

![MVVM diagram](https://user-images.githubusercontent.com/3226755/94297948-9f713e00-ff65-11ea-9dce-f44bbef708a6.png)

* The **model** is any raw data that your game should display to the user. It could be player status, enemies status, systems information, configuration, etc.

* The **view** is a passive interface that displays data (the model) and routes user commands (events) to the viewmodel to act upon that data. It is typically a GameObject built of uGui components with a main class to handle them.

* The **viewmodel** is an intermediary class that retrieves data from the model and formats it for display in the view. It also reacts to user events in the view and updates the model.

jUIce contains some base classes and systems that implement this pattern to let you focus on your game's concrete needs.

## UIFrame
jUIce's view hierarchy is organized below a root element called the UIFrame. It contains a series of layers to sort your views based in certain rules and acts as the service to handle the views' visibility. Before opening any view, it needs to be registered in the UIFrame.

```csharp
using Juice;
using UnityEngine;

public class ViewController : MonoBehaviour
{
    [SerializeField] private UiFrame uiFrame;
    [SerializeField] private MyView myView;

    private void Start()
    {
        // A view needs to be registered before being shown
        uiFrame.Register<MyView>(myView);
    }

    private void Update()
    {
        if (Input.KeyDown(KeyCode.Escape))
        {
            if (myView.IsVisible)
            {
                uiFrame.Hide<MyView>().Execute();
            }
            else
            {
                uiFrame.Show<MyView>().WithViewModel(new MyViewModel()).Execute();
            }
        }
    }
}
```

Your typical UiFrame will contain a set of layers that will define the order in which your views will be sorted. 

The default layer stack that jUIce can automatically build for you would be something like this:
1. **Panel Layer** (Panels with no priority, usually HUD elements)
2. **Window Layer** (Regular windows)
3. **Priority Panel Layer** (Panels set to `Prioritary`, shown on top of regular windows)
4. **Priority Window Layer** (Popups)
5. **Overlay Panel Layer** (Panels shown on top of popups)
6. **Tutorial Panel Layer** (To overlay your UI with tutorial elements)
7. **Blocker Layer** (Highest priority windows, like loading screens or connectivity issues displayers)

Keep in mind that this is the default hierarchy. It should be enough for most of the games but you are free to arrange it in some other order or even create your custom layers to fit your game needs.

## View
There are two kinds of views in jUIce: Windows and Panels. The main reason for this distinction is their overall behavior and conceptual meaning; windows are the focal element of information for the user and there can only be one of them focused at any given moment, whereas any number of panels can be open at the same time and alongside the current window so they work as containers for complementary info.

### Window
A window is the focal element of UI information at any given time (if any). They usually take up all the space available in the screen and, therefore, only one of them can be focused in a particular moment. They are stored in the history stack and will be automatically shown again with the same ViewModel when the next window in the stack is closed.

According to their behavior, there are two kinds of window: regular and popup. 

A **regular window** is your main source of dialog with the player. They usually take all the screen space and conform the menu tree of your game.

A **popup**, on the other hand, is a volatile kind of window that is supposed to be shown over the current displayed views (both windows and panels). They are automatically shown over a background shadow to occlude previous information.

Both of them can be enqueued so they are shown in order when the previous one is closed. 

### Panel
A panel is a block of UI information which is not bound to any particular window. There can be as many panels shown at the same time as your game needs and, because of that, they usually contain complementary information that can outlive windows after they are hidden.

## Transitions
A UI without subtle animations is like a muffin without topping. You'll want your views to be animated when they transition into a visible or invisible state. That's achieved with `Transition`s.

Transitions are `Component`s that can be attached to any `View` in order to define how will it behave when a `Show` or `Hide` operation is requested on it. `ShowTransition` and `HideTransition` are independent fields in the `View`'s editor, so you can set different behaviors based on the direction of the transition. If no transition is set for a particular operation, the View's GameObject will be just activated/deactivated when requested.  

There is a set of common transitions already included in jUIce's codebase, but you can create your own if there's none that satisfy your UI/UX requirements.

## Observables
The observable family is the bread and butter of jUIce. They act as the glue that keeps everything interconnected.

The View and the ViewModel communicate with each other through observable objects. These classes have mechanisms to automatically notify observers about changes in their internal state.

There are four members in the observable family, each of them intended for a particular need in the system. 

### ObservableVariable
This is the battle horse of the whole system; one of the simpler and most useful of all the members of the family. It just wraps a variable and notifies when its value changes. It's worth noting that it won't raise a change event when its `.Value` is set with the same value that is already stored.

### ObservableCollection
It represents a collection or a list of elements of the same type. You'll find all operations that you'd expect for a regular `List<T>`, and it'll notify a particular event for each of them.

The `ObservableCollection` grants you a lot of control when dealing with collections of data and provides an extensive pool of relevant information about the events that take place for those entities listening for changes in its contents.

### ObservableCommand
The `ObservableCommand` is the channel through which the view can communicate with the ViewModel when the user performs an action with the intention to change the underlying model.

In addition to an `.Execute()` method to request an action by the ViewModel, it also exposes an `ObservableVariable<bool>` that tells the requester whether said action can be performed or not in this particular moment. This is really useful to give the user feedback about the available actions they have at their disposal, by greying out buttons when `.CanExecute.Value` is `false`, for example.

There are two versions of `ObservableCommand`, one of them with a generic parameter `T`, so you can add some information about the requested action.

### ObservableEvent
The last member of the family and the most obvious of them all. An `ObservableEvent` is just... and event; something that takes place in a particular moment and which value (if any) is not supposed to be stored to be consulted in the future. 

Like the `ObservableCommand`, the event has a generic version that can be used to supply additional information about the observed happening.

## ViewModel
The `ViewModel` is some sort of _translator_ between your business model and the view. It holds all the data that the view requires in a simple and easy to consume format. 

For that matter, it exposes Observable objects and keeps them up to date according to changes in the business model. Included in those observable objects, there could be `ObservableCommand`s, which are the way the View is able to communicate user driven events to the ViewModel so it can process them and update the model accordingly.

The ViewModel is **not** a `MonoBehaviour`, meaning that it can be passed away between classes in the jUIce system and it doesn't necessarily live on a concrete `GameObject`. 

Creating a new ViewModel is pretty straightforward. You can extend the `ViewModel` class, which already handles a couple of mechanism for you and allows your code to be easily attached to the game's update loop or react to some events during the ViewModel's lifecycle. 

```csharp
using Juice;

public class MyViewModel : ViewModel
{
    public IReadonlyObservableVariable<float> MyVariable => myVariable;
    public IObservableCommand MyCommand => myCommand;
    public IObservableEvent MyEvent => myEvent;

    private ObservableVariable<float> myVariable;
    private ObservableCommand myCommand;
    private ObservableEvent myEvent;

    private MyModel model;

    // You can freely use constructors
    public MyViewModel(MyModel model)
    {
        myVariable = new ObservableVariable<float>(myModel.myVariable);

        myCommand = new ObservableCommand();
        myCommand.ExecuteRequested += OnMyCommandExecuteRequested;

        myEvent = new ObservableEvent();

        this.model = model;
    }

    // Called when the ViewModel starts being used
    protected override void OnEnable()
    {
        model.NotifiedSomething += OnMyModelNotifiedSomething;
    }

    // Called when the ViewModel is put on hold and not used anymore
    protected override void OnDisable()
    {
        model.NotifiedSomething -= OnMyModelNotifiedSomething;
    }

    // Update works as MonoBehaviour's Update method
    protected override void Update()
    {
        myVariable.Value = model.myVariable;
    }

    private void OnMyCommandExecuteRequested()
    {
        model.DoSomething();
    }

    private void OnMyModelNotifiedSomething()
    {
        myEvent.Raise();
    }
}
```

You are free to create your ViewModel from scratch instead of extending `ViewModel`. The only restriction is that you need to implement the `IViewModel` interface for it to be used by the framework.

It's worth mentioning that the ViewModel is the entry point for everything that's gonna happen in the View so it is a good idea to keep it as simple and as clean as possible. Of course, you should always keep in mind that the ViewModel is not responsible for anything related to the way the View is displaying its data. It should only provide data, not styles or behaviours. 

## Bindings
Bindings are the last link the framework chain. They are responsible of providing updated information about changes in the ViewModel to the Unity Components that use them.

There are matching binders for every member of the observable family and some other elements to ease development and displaying the information in the Editor.

### Binders
Binders are the actual components (`MonoBehaviour`s) that operate over a Unity `Component` so the View can reflect the internal state of the ViewModel.

jUIce includes many binders for uGui's components as well as some other collections that let your UI objects react to changes on the ViewModel.

### OperatorBinders
These are a special kind of binders. Binders that can process one or more values from the ViewModel and expose a derived value based on its input. 

This mechanism grants jUIce an important expressive power, allowing the designers and UI artists to mix and match properties to create their own to fit the requirements of the View. Remember that the View can (and will) constantly change as the project grows and evolves, and having a clear and strict separation between the ViewModel (the data to be displayed) and the View (how is that data displayed) is a guarantee for a more agile development process.

OperatorBinders are heavily inspired by [ReactiveX](http://reactivex.io/documentation/operators.html) operators, so you can expect to find many of the most valuable operations from that library. 

## Additional Goodies
The framework also include a couple of subsystems that could be used in your non UI modules. 

### Signals
`SignalBus` is a lightweight event dispatcher that lets you have independent signal channels to be shared between objects as well as a default bus that can be accessed globally.

### Tweening
There's also a simple tween library designed to mimic the amazing [DOTween](http://dotween.demigiant.com/) plugin, which is used to add many tweening features in the framework.
