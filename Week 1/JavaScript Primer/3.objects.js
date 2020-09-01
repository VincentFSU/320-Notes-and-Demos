const obj = 
{
    x:13
}

obj.x = 57;

console.log(obj.x);
obj.update = function()
{
    console.log("wow, I'm updating")
};

obj.update();

function Person()
{
    this.name = "Jimmy";
    this.sayHello = ()=>
    {
        console.log("Hello, I'm " + this.name);
    }
}

var jim = Person();


// ES 6 introduced classes to JS


// declare properties inside of a constructor, or they won't scope correctly
class Sprite
{
    constructor()
    {
        this.x = 0;
        this.y = 154;
        this.rotation = 45;
    }

    die()
    {
        this.isDead = true;
    }
}

class Enemy extends Sprite
{
    constructor()
    {
        //MUST call the superclass's constructor before anything else
        super();
    }

    spin(amount)
    {
        this.rotation += amount;
    }
}

var e = new Enemy();

console.log(e.rotation);
e.spin(60);
console.log(e.rotation);