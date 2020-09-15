// sometimes 'this' is mapped onto other objects
// specifically, when using event-listeners

// 'this' gets mapped as the event object (not global)
setTimeout(function(){ console.log(this) }, 100);

// arrow functions do NOT change the context of 'this':
setTimeout(()=>{ console.log(this) }, 100);

class Server {
    constructor(){
        
        this.port = 1234;

        // 'this' will point to the createServer even 
        const sock = require('net').createServer({}, function(){
            console.log(this.port);
        });

        // 'this' will point to the port as the arrow function does NOT change the context of 'this'
        const sock = require('net').createServer({}, ()=>{
            console.log(this.port);
        });
    }
}