// how does the 'this' keyword work?

//in oop, 'this' refers to the object that's containing the code we are running.

const cat = "meow"; // global variable

function doSomething(){
    console.log(cat); // works because cat is in the global scope.
}

// doSomething();

function example1(){
    console.log(this);
}

new example1();

class Example2{
    constructor(){
        console.log(this);
    }
}

new Example2();