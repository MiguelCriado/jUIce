# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
This version changes radically the way the framework works and how is it used. 

### Added
- A MVVM system.
- Binding mechanisms. 
- Operator Binder base class.
- CombineLatest operator.
- Delay operator.
- Map operator.
- Merge operator.
- Scan operator.
- Skip operator.
- StartWith operator.
- Take operator.
- To operator.
- Tween operator.
- ConstantBindings. 
- Observable Event.
- A View creation wizard. 
- Built-in transitions. 
- Tweening library.
- GameObject pooling library. 

### Changed
- Project name from "Maui" to "Juice".
- The main control pattern. Now the framework features a MVVM approach. (This is **HUGE**).
- View are now registered as instances instead of prefabs.
- Popup's background is now a Widget instead a fixed shadow.
- Now ObservableVariable's value is not set on creation. 

## [0.1.6] - 2020-05-02
### Added
- Event initialization in ObservableCommand's constructor.
- PresenterInitializer class.
- CreateMenu entries to automatically create new Screen class files.

### Changed
- Project structure from Package to Plugin.
- Event naming to follow .NET conventions.
- Project name from Muui to Maui.
- Root namespace from Muui to Maui.

### Fixed
- A compilation error when targeting .Net Standard 2.0.
- NullReferenceExceptions in UIFrame hierarchy when initializing Presenters on Awake.

## [0.1.5] - 2020-02-20
### Added
- A try/catch clause to Transition.Animate calls.
- WhileHiding method to allow Screens to react to Hide actions.

### Changed
- The behavior of the popup shadow to hide while closing the last one.

## [0.1.4] - 2020-02-18
### Added
- Widget element.
- A fade transition to Popups's background shadow when shown/hidden.
- HideScreen method to BasePresenter.
- A new property for Popups to be closed on background shadow click.
- Method in BaseTransition to prepare animations.
- Pragma directives to avoid warnings in projects.

### Changed
- Now the screens are disabled immediately after registration.
- Transitions return a Task instead of callbacks on termination. 

## [0.1.3] - 2020-02-16
### Added
- SignalBus class as an event dispatch mechanism.

## [0.1.2] - 2020-02-12
### Fixed
- OnPropertiesSet call of BaseWindowController subclasses when properties are set.

## [0.1.1] - 2020-02-11
### Fixed
- Screen registration in layers.

## [0.1.0] - 2020-02-11
### Added
- ObservableVariable.
- ObservableCollection.
- ObservableDictionary.
- ObservableCommand.

## [0.0.2] - 2020-02-06
### Added
- CHANGELOG.md file. 

### Fixed
- Package name to comply to the naming conventions of Unity Packages.

## [0.0.1] - 2020-02-06
### Added
- Muui Framework. The barebones of a basic UI framework to be used atop Unity's uGui.
- Basic Unit Test cases to prove the system works.
