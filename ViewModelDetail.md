## Interection with View
ViewModel's properties should be set in a way that the View can directly bind to without using any Converter. Our architecture don't use Converter.
Sometimes, ViewModel need to have reference to the View. For example some element have its width equal to 1/3 of the View's width. Since we don't use Converter, 


## Interection with Model
By how it interect with Model, ViewModels can be classified into 4 types:

### Instructive (Bidirectional):
ViewModel does not have all data that Model will be.
It sends some data to Model and let it update itself, then retrieve data from it.

### Dominative (Writing only):
ViewModel have all data that Model will be.
It sends data to Model but don’t need to retrieve data from it.

### Listening (Reading only):
ViewModel don’t send any data to Model and just retrieve data from it.

### Standalone (No Model):


## Patterns of Implementation

For ViewModels that involve writing the Model, reference to the Model should be saved as public property with private set.
They should also have a method named as "AssignModel" and takes one or several arguments to set the Model property.

If there are other ViewModels that should respond to the change of Model,
it call UpdateDataFromModel method of ViewModels under it.
Or call the Send method of the WeakReferenseMessenger.

If WeakReferenceMessenger is to be used, an additional message class should be specifically defined.
