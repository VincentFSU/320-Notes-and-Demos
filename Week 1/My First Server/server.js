const net = require('net');

const socketServer = net.createServer(socketClient=>
{
    console.log("new connection!");
    socketClient.on("error", errMsg=>
    {
        console.log("error: " + errMsg);
    });

    socketClient.write("Hello! Welcome to the server!");
    socketClient.on("data", txt=>
    {
        console.log("message from client: "+txt);
    });
});

//We will have one socket dedicated to listening for new connections
socketServer.listen({port:12345}, ()=>
{
    console.log("listening for connections...");
});

socketServer.on("error", (errMsg)=>
{
    console.log("didn't work: " + errMsg);
});