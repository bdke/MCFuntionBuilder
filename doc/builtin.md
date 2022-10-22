# BuiltIn Functions and Classes
## Functions
### Program
#### Write
```
Write(4);
```
Output `4` to console
#### LoadFile
```
var string = LoadFile("path/to/file");
```
Return text inside the file
#### Execute
```
Execute(fileString);
```
It will execute the code if it is a valid `C#` code with `Main` method
## Classes
### Program
### Minecraft Commands
#### Function
***Static class***
```
Function.Call("namespace","path/to/mcfuntion");
```
Equavilent to
```
function namespace:path/to/mcfuntion
```
---
```
Function.Create("namespace","path/to/name")
```
It will create a `name.mcfuntion` in `path/to`  
Return a `FunctionData` class

---
```
Function.GetCurrentNamespace()
```
return a `string` of the current namespace
```
Function.GetCurrentFunction()
```
return a `string` of the current function

---
#### FunctionData
***Non-Static***  
Not yet Implemented
#### Selector
***Non-Static***  
```
var selector = new Selector("@s")
```
Constructor must include a `Selector` string

May add more methods afterward
