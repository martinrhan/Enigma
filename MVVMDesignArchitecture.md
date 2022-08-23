###Model
The basic dataset that everything depend on. It should be completely independent from ViewModel application. Model can be any class. It is usually from assembly other than the UI application, but some models are within the UI assembly.

###RegularModel
Sometimes, multiple ViewModels are reading from same model concurrently. If one of them write to the model, other ViewModels are not notified about the change of model.
RegularModels and Models are one-one paired and RegularModel's reference to Model is readonly.
Else than the RootView, only RegularModels can access Model directly.

###ViewModel
Act as translator and adaptor between RegularModel and View. Unlike one-one paired reference of View and ViewModel, relation between ViewModels and RegularModels is allowed to be zero or multiple, and allowed to be changed any times.
A ViewModel always have a View, however, it should only read from View.
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

###View
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

###RootView
Creates first level Views, ViewModels, and Models, then assign them accordingly.

Generaly, the Application have their Views, ViewModels, and Models are orgranized into three trees respectively. They share the same root, that is the RootView, and each node of the three parallel tree has a corresponding node in other two trees.

###Entries
An entry is where changes the state of application it can be either changing View and ViewModel only, or also changes RegularModel and Model behind it. It is usually an user input. If the model can mutate itself without user control, then the application probably have an entry that is a periodic reading of Model (of course this is done through ViewModel and RegularModel).
An entry is always launched at UI layer, which is the View. The View then call its ViewModel's function or set its property. Here comes the most tricky part: the implementation of ViewModels.

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
	<v:ChildViewA ViewModel="Binding ViewModel.ChildViewModelA, ElementName=view">
	<v:ChildViewB ViewModel="Binding ViewModel.ChildViewModelB, ElementName=view">
</v:View>
````
While its ViewModel is like this:
```
public class MyViewModel : ViewModel{
	public ChildViewModelA ChildViewModelA{get;} = new ChildViewModelA();
	public ChildViewModelB ChildViewModelB{get;} = new ChildViewModelB();
}
```
