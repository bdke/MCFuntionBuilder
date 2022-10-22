# Syntax
This will include all syntaxes of the mcfbuilder
## Files
`#foo`  
The code after this line will be written to `bar.mcfuntion`  
  
`#foo/bar`  
The code after this line will be written to `foo/bar.mcfuntion`
## Variables
***use `global` instead of `var` if the variable need to be accessed outside of the function***  
  
### Normal Variables
`var foo = "bar";`   
Declaring variable `foo` with a value of `"bar"` 

### Scoreboard Variables
`score var foo @s = 3;`  
Declaring a `dummy` scoreboard `foo` with value `3` for `@s` player  
equavilent to  
```mcfunction
scoreboard objectives add foo dummy
scoreboard players set @s foo 3
```  
At the end of function (if `var` is used)
```mcfunction
scoreboard objectives remove foo
```
---
`score var foo @s = "health";`   
Declaring a `health` scoreboard `foo`  
equavilent to  
```mcfunction
scoreboard objectives add foo health
```  
At the end of function (if `var` is used)
```mcfunction
scoreboard objectives remove foo
```

---
`foo @s = 2;`
```mcfunction
scoreboard players set @s foo 2
```  
`foo @s += 2;`
```mcfunction
scoreboard players add @s foo 2
``` 
`foo @s -= 2;`
```mcfunction
scoreboard players remove @s foo 2
``` 
`foo @s *= 2;`
```mcfunction
scoreboard players set @s .number 2
scoreboard players operation @s foo *= @s .number
``` 
`foo @s /= 2;`
```mcfunction
scoreboard players set @s .number 2
scoreboard players operation @s foo /= @s .number
``` 
`foo @s %= 2;`
```mcfunction
scoreboard players set @s .number 2
scoreboard players operation @s foo %= @s .number
``` 

---
`foo @s = bar @s;`
```mcfunction
scoreboard players operation @s foo = @s bar
```  
`foo @s += bar @s;`
```mcfunction
scoreboard players operation @s foo += @s bar
``` 
`foo @s -= bar @s;`
```mcfunction
scoreboard players operation @s foo -= @s bar
``` 
`foo @s *= bar @s;`
```mcfunction
scoreboard players operation @s foo *= @s bar
``` 
`foo @s /= bar @s;`
```mcfunction
scoreboard players operation @s foo /= @s bar
``` 
`foo @s %= bar @s;`
```mcfunction
scoreboard players operation @s foo %= @s bar
``` 

### Tag Variables
`tag var foo @s = true;`  
Equvilent to 
```mcfunction
tag @s add foo
```
At the end of function (if `var` is used)
```mcfunction
tag @s remove foo
```
  
`foo @s = false;`   
Equvilent to 
```mcfunction
tag @s remove foo
```
## Iteration
```
for (i in 1..5) {
    foo @s = false;
}
```
execute code 4 time inside the `for` loop
```
for (i in ["foo",bar]) {
    var egg = i;
}
```
loop over the list or dictionary
```
while (foo == 3) {
    var egg = "chicken";
}
```
while loop
## Conditional
```
if (bar == 3) {
    /say egg
}
```
`bar` must be a normal variable

---
#### Entity
```
if entity (@s) {
    /say chicken
}
```
`condition` must be a `string` or `selector`  
  
Equavilent to  
```
execute if entity @s run say hi
```

---
### ***<span style="color:red; ">!Not Implmented</span>***
#### Score
```
if score (foo @s > bar @s) {
    /say eggggggg
}
```
`foo` and `bar` must be a scorebaord variables  
  
Equavilent to
```
execute if score @s foo > @s bar run say egggggggg
```
  
#### Predicate
```
if predicate ("namespace:path/to/predicate") {
    /say hi
}
```
Equavilent to
```
execute if predicate namespace:path/to/predicate run say hi
```

#### Block
```
if block ~ ~ ~ ("grass_block") {
    /say block
}
```
  
Equavilent to
```
execute if block ~ ~ ~ grass_block run say block
```

#### Blocks
```
if blocks ~ ~ ~ ~ ~ ~ ~ ~ ~ all {
    /say blocky
}
```
Equavilent to
```
execute if blocks ~ ~ ~ ~ ~ ~ ~ ~ ~ all run say blocky
```

#### Data
```
if data entity @s ("SpawnX") {
    /say spawn
}
```
Equavilent to
```
execute if data entity @s SpawnX run say spawn
```
---
More information on [FandomWiki](https://minecraft.fandom.com/wiki/Commands/execute)
## Function
```
def foo(value) {
    return value;
}
```
Create a function with `def` keyword and `return` a value
Can be called by `foo(4);`

## Class
### Non-Static
```
var foo = new Bar(3);
```
Create a new class `Bar` with constructor inputted with value `3`  
Call method of `Bar` by `foo.Foo();`
### Static
```
var foo = Bar.Foo();
```
Calling a static method




