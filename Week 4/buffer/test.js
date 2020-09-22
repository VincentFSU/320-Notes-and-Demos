const net = require("net");


const Socket = net.connect({port:320, ip:"127.0.0.1"}, ()=>{
    console.log("connected to server...");

    const buff = Buffer.alloc(10);
    buff.write("JOIN"); // identifier
    buff.writeUInt8(5, 4); // length of name, offset
    buff.write("Vince", 5); // name, offset
    
    Socket.write(buff);

});

Socket.on("error", e=>{
    console.log("ERROR: "+e);
})