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
*TODO*

### Window
*TODO*

### Panel
*TODO*

## Observables
The observable family is the bread and butter of Maui. They act as the glue that keeps everything interconnected.

The view and the viewmodel communicate with each other through observable objects. These classes have mechanisms to automatically notify observers about changes in their internal state. 
*TODO*

## ViewModel
*TODO*

## Bindings
*TODO*

## Best Practices
*TODO*
