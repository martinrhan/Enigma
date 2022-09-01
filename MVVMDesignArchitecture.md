## Model
The basic dataset that everything depend on. It should be completely independent from UI. It can be any class. 
It is usually from assembly other than the UI, but some models are within the UI assembly.

## ViewModel
Act as translator and adaptor between Model and View.

ViewModels are one-one paired with Views.
A View always have reference to a ViewModel and this reference should not be changed after first set.
A ViewModel can optionaly have reference to its View. However this is not recommended and should only be done when necessary.
Furthermore, ViewModel should only ready from View.

If a ViewModel need to write to model it should have reference to its model.

If a ViewModel need to read from model it should have a method of void return type named UpdateDataFromModel, parameter can be any number of any type as long as they are models.

There are two patterns of callng UpdateDataFromModel.
Calling from higher level of the ViewModel hierachy, or calling through WeakReferenceMessenger.

All ViewModel inherits following abstract class:
```
public abstract class ViewModel : INotifyPropertyChanged {
	public event PropertyChangedEventHandler PropertyChanged;
	protected void NotifyPropertyChanged([CallerMemberName] string propertyName = null) {
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
	public bool IsEmpty { get; set; }
}
```

## View
Where UI is. It must have a ViewModel and able access it, but unable to access Model directly. Views and ViewModels are one-one paired. Their reference to each other should only be set upon initialization.
All Views inherits Following class:
```
public class View<T> : ContentControl, IView<T> {
	public T ViewModel {
		get { return (T)GetValue(ViewModelProperty); }
		set { SetValue(ViewModelProperty, value); }
	}
	public static readonly DependencyProperty ViewModelProperty =
		DependencyProperty.Register(nameof(ViewModel), typeof(T), typeof(View<T>), new PropertyMetadata());
}
public interface IView<out T> {
	public T ViewModel { get; }
}
```

## RootView
Creates first level Views, ViewModels, and Models, then assign them accordingly.
It is usually a Window, or a Page. It can also be something else similar.

Generaly, the Application have their Views, ViewModels, and Models orgranized into three trees respectively.
They share the same root, that is the RootView, and each node of the three parallel tree has corresponding nodes in other two trees.

## Entries
An entry is where changes the state of application.
It can be either changing View and ViewModel only, or also changes Model behind it.
It is usually an user input. 

If the model can mutate itself without user's instruction,
then the application probably should have an entry that is a periodic reading of Model
(of course this is done through ViewModel and RegularModel).

An entry is always launched at UI layer, which is the View.
The View then call its ViewModel's function or set its property.

## Implementaion Example
A typical View has its xaml looks like this:
```
<v:View x:TypeArguments="local:MyViewModel" x:Name="view"
    x:Class="MyWPF.Visual.MyView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006" 
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008" 
             xmlns:v ="clr-namespace:MyWPF.Visual"
             xmlns:local="clr-namespace:MyWPF.Visual"
             mc:Ignorable="d" 
             d:DesignHeight="300" d:DesignWidth="200">
	<v:ChildAView ViewModel="Binding ViewModel.ChildAViewModel, ElementName=view">
	<v:ChildAView ViewModel="Binding ViewModel.ChildBViewModel, ElementName=view">
</v:View>
````
While its ViewModel is like this:
```
public class MyViewModel : ViewModel{
	public ChildAViewModel ChildAViewModel { get; } = new ChildAViewModel();
	public ChildBViewModel ChildBViewModel { get; } = new ChildBViewModel();
}
```

