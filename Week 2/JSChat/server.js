const net = require("net"); //import nodejs TCP socket module
const { listenerCount } = require("process");
const clients = [];

const listeningSocket = net.createServer({}, (socketToClient)=>{
    console.log(socketToClient.localAddress + " has connected!");
    clients.push(socketToClient);

    socketToClient.on("error", (errMsg)=>{
        console.log("ERROR: " + errMsg);
    });

    socketToClient.on("close", ()=>{
        console.log("A client has disconnected...");
        clients.splice(clients.indexOf(socketToClient), 1);
        console.log(clients.length);
    });

    socketToClient.on("data", txt=>{
        console.log("Client says: " + txt);
        BroadcastToAll(txt, socketToClient);
    });

    socketToClient.write("Welcome to my server. \n")
});

listeningSocket.listen({port:320}, ()=>{
    console.log("The server is now listening for incoming connections...");
});

function BroadcastToAll(txt, socketToSkip)
{
    clients.forEach(client=>{
        //if (socketToSkip != client)
        client.write((clients.indexOf(client) + 1) + ": " + txt);
    });
}


