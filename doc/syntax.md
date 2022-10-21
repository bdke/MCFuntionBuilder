# Syntax
This will include all syntaxes of the mcfbuilder
## Basic Syntax
### Files
<code>
    <span class="string">#bar</span>
</code>

The code after this line will be written to `bar.mcfunction`

<code>
    <span class="string">#foo/bar</span>
</code>

The code after this line will be written to `./foo/bar.mcfunction`

### Variables
#### User Defined Variables
<code>
    <span class="keyword">var</span>
    <span class="id">foo</span>
    <span class="operator">=</span>
    <span class="string">"bar"</span>
    ;
</code>
</br></br>
<p>Variables can be declared in a javascript way</p>

#### Scoreboard Dummy
<code>
    <span class="keyword">score</span>
    <span class="keyword">var</span>
    <span class="id">foo</span>
    <span class="selector">@s</span>
    <span class="operator">=</span>
    <span class="int">1</span>
    ;
</code>
</br></br>
<p>
    It will declare a <code>dummy</code> scoreboard with value 
        <code><span class="int">1</span></code>
    </br>
    equavilent to 
    </br>
    </br>
    <code>
        scoreboard objectives add foo dummy
    </code>
    </br>
    </br>
    <code>
        scoreboard players set @s foo 1
    </code>
    </br>
    </br>
    At the end of function:</br>
    <code>
        scoreboard objectives remove foo
    </code>
    </br>
    </br>
    declaring value and selector is optional
</br>

#### Tag
<code>
    <span class="keyword">tag</span>
    <span class="keyword">var</span>
    <span class="id">foo</span>
    <span class="selector">@s</span>    
    <span class="operator">=</span>
    <span class="keyword">true</span>
    ;
</code>
</br>
It will give <code><span class="selector">@s</span></code> a tag <code><span class="id">foo</span></code>
equavilent to 
</br>
</br>
<code>
    tag @s add foo
</code>
</br>
</br>
At the end of function:</br>
<code>
    tag @e remove foo
</code>
</br>
</br>
declaring value and selector is optional
<h4><b>
    Be reminded that using <code><span class="keyword">var</span></code> will destruct the variables after End Of Function</br>
    Use <code><span class="keyword">global</span></code> instead if you want to access it outside the Function
</b></h4>
