// require module:
const dgram = require("dgram");

// make socket:
const sock = dgram.createSocket("udp4");

// setup event-listeners:
// sock.on("", ()=>{});
sock.on("error", (e)=>{
    console.log("ERROR: " + e);
});
sock.on("listening", ()=>{
    console.log("Server listening...")
});
sock.on("message", (msg, rinfo)=>{
    console.log("--- packet received ---");
    console.log("from "+ rinfo.address +" : " + rinfo.port)
    console.log(msg.toString());
});

// start listening:
sock.bind(320);