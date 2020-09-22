const Client = require("./client").Client;

exports.Server = {
    port:320,
    clients:[],
    start(){
        this.socket = require("net").createServer({}, c=>this.onClientConnect(c));
        this.socket.on("error", e=>this.onError(e));
        this.socket.listen({port:this.port}, ()=>this.onStartListen());
    },
    onClientConnect(socket){
        console.log("New connection from " + socket.localAddress);
        
        // instantiate clients
        const client = new Client(socket, this);
        this.clients.push(client);
    },
    onClientDisconnect(client){
        const index = this.clients.indexOf(client);
        if(index >= 0) this.clients.splice(index, 1); //remove from array. Negative if can't find.
    },
    onError(e){
        console.log("ERROR with listener: "+e);
    },
    onStartListen(){
        console.log("Server is now listening on port " + this.port);
    },
};
// the object that can be referenced with "require"

console.log("server.js is running...");