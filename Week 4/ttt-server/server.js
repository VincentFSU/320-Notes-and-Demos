const Client = require("./client").Client;
const PacketBuilder = require("./packet-builder.js").PacketBuilder;

exports.Server = {
    port:320,
    clients:[],
    maxConnectedUsers:8,
    start(game){
        this.game = game;
        this.socket = require("net").createServer({}, c=>this.onClientConnect(c));
        this.socket.on("error", e=>this.onError(e));
        this.socket.listen({port:this.port}, ()=>this.onStartListen());
    },
    onClientConnect(socket){
        console.log("New connection from " + socket.localAddress);

        if(this.isServerFull()){ // server is full
            
            const packet = PacketBuilder.join(9);
            socket.end(packet);

        } else { // server is not full
            // instantiate clients
            const client = new Client(socket, this);
            this.clients.push(client);
        }
        
        
    },
    onClientDisconnect(client){
        if(this.game.clientX == client) this.game.clientX = null;
        if(this.game.clientY == client) this.game.clientY = null;
// TODO : select a spectator to take over

        const index = this.clients.indexOf(client);
        if(index >= 0) this.clients.splice(index, 1); //remove from array. Negative if can't find.
    },
    onError(e){
        console.log("ERROR with listener: "+e);
    },
    onStartListen(){
        console.log("Server is now listening on port " + this.port);
    },
    isServerFull(){
        return (this.clients.length >= this.maxConnectedUsers);
    },
    generateResponseID(desiredUsername, client){
        //this function returns a response id

        if (desiredUsername.length < 3) return 4; // username too short!
        if (desiredUsername.length > 16) return 5; // username too long!

        const regex1 = /^[a-zA-Z0-9]+$/; // literal regex in JavaScript
        if(!regex1.test(desiredUsername)) return 6; // uses invalid characters

        let isUsernameTaken = false;
        this.clients.forEach(c => {
            if(c == client) return;
            if(c.username == desiredUsername) isUsernameTaken = true;
        });

        if(isUsernameTaken) return 7; // username taken

        const regex2 = /(fuck|fvck|fuk|shit|damn|faggot|nigger|cunt|bitch|spic)/i;
        if(regex2.test(desiredUsername)) return 8; // username contains profanity

        if(this.game.clientX == client) {
            return 1; // keep as clientX
        } 
        
        if(this.game.clientO == client) {
            return 2; // keep as clientO
        }

        if(!this.game.clientX) {
            this.game.clientX = client;
            return 1; // set as clientX
        } 
        
        if(!this.game.clientO) {
            this.game.clientO = client;
            return 2; // set as clientO
        }

        return 3; // set as spectator
    },
    broadcastPacket(packet){
        this.clients.forEach(c=>{
            c.sendPacket(packet);
        })
    }
};
// the object that can be referenced with "require"

console.log("server.js is running...");