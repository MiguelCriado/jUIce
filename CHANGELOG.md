# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Fixed
- A compilation error when targeting .Net Standard 2.0.

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