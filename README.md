# muui
Muui is a framework built on top of Unity's uGui solution. It provides a series of systems and guidelines to boost your UI development within Unity.

This project is based on the amazing [deVoid UI Framework](https://github.com/yankooliveira/uiframework) by Yanko Oliveira. 

## Framework's Philosophy
The framework encourages a [Model-View-Presenter](https://en.wikipedia.org/wiki/Model%E2%80%93view%E2%80%93presenter) approach, splitting the main concerns of any UI piece into a couple of classes that interoperate together.

![Model-View-Presenter](https://user-images.githubusercontent.com/3226755/79693197-8ef7c180-8269-11ea-8e05-f01ad7d0d39a.png)

* The **model** is any raw data that your game should display to the user. It could be player status, enemies status, systems information, configuration, etc.

* The **view** is a passive interface that displays data (the model) and routes user commands (events) to the presenter to act upon that data. It is tipically a GameObject built of uGui components with a main class to handle them.

* The **presenter** is an intermediary class that retrieves data from the model and formats it for display in the view. It also reacts to user events in the view and updates the model.

Muui contains some base classes and systems that implement this pattern to let you focus on your game's concrete needs.

## UIFrame
*TODO*

## Screen
*TODO*

## Presenter
*TODO*

## Best Practices
*TODO*
