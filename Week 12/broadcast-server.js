const sock = require('dgram').createSocket('udp4');

sock.on("listening", ()=>{
    console.log("now listening...");
});
sock.on("message", ()=>{
    console.log("packet received!");
});

sock.bind(320);