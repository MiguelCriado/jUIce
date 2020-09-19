# Maui
Maui is a framework built on top of Unity's uGui solution. It provides a series of systems and guidelines to boost your UI development within Unity.

This project is based on the amazing [deVoid UI Framework](https://github.com/yankooliveira/uiframework) by Yanko Oliveira. 

## Framework's Philosophy
The framework encourages a [Model-View-ViewModel](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93viewmodel) approach, splitting the main concerns of any UI piece into a couple of classes that interoperate together.

![Model-View-ViewModel](https://user-images.githubusercontent.com/3226755/79693197-8ef7c180-8269-11ea-8e05-f01ad7d0d39a.png)

* The **model** is any raw data that your game should display to the user. It could be player status, enemies status, systems information, configuration, etc.

* The **view** is a passive interface that displays data (the model) and routes user commands (events) to the viewmodel to act upon that data. It is typically a GameObject built of uGui components with a main class to handle them.

* The **viewmodel** is an intermediary class that retrieves data from the model and formats it for display in the view. It also reacts to user events in the view and updates the model.

Maui contains some base classes and systems that implement this pattern to let you focus on your game's concrete needs.

## UIFrame
Maui's view hierarchy is organized below a root element called the UIFrame. It contains a series of layers to sort your views based in certain rules and acts as the service to handle the views' visibility. Before opening any view, it needs to be registered in the UIFrame.
*TODO*

## View
There are two kinds of views in Maui: Windows and Panels. The main reason for this distinction is their overall behavior and conceptual meaning; windows are the focal element of information for the user and there can only be one of them focused at any given moment, whereas any number of panels can be open at the same time and alongside the current window so they work as containers for complementary info.

### Window
*TODO*

### Panel
*TODO*

## Observables
The observable family is the bread and butter of Maui. They act as the glue that keeps everything interconnected.

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

The ViewModel is **not** a `MonoBehaviour`, meaning that it can be passed away between classes in the Maui system and it doesn't necessarily live on a concrete `GameObject`. 

Creating a new ViewModel is pretty straightforward. You can extend the `ViewModel` class, which already handles a couple of mechanism for you and allows your code to be easily attached to the game's update loop or react to some events during the ViewModel's lifecycle. 

```csharp
using Maui;

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
*TODO*

## Best Practices
*TODO*
