###Interection with View
ViewModel's properties should be set in a way that the View can directly bind to without using any Converter. Our architecture don't use Converter.
Sometimes, ViewModel need to have reference to the View. For example some element have its width equal to 1/3 of the View's width. Since we don't use Converter, 


###Interection with RegularModel
By how it interect with RegularModel, ViewModels can be classified into 4 types:
-Instructive (Bidirectional): ViewModel does not have all data that Model will be. It sends some data to RegularModel and let it update itself, then retrieve data from it.
-Dominative (Writing only): ViewModel have all data that Model will be. It sends data to RegularModel but don’t need to retrieve data from it.
-Listening (Reading only): ViewModel don’t send any data to Model and just retrieve data from it.
-Standalone (No RegularModel)
Programmers should follow conventions stated below.
For ViewModels that involve reading the RegularModel, a reference to the RegularModel should be saved as a private field. They should also have a method named as "AssignModel".
