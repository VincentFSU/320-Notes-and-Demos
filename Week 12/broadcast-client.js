const sock = require('dgram').createSocket('udp4');

const packet = Buffer.from("hello world!");

sock.send(packet, 0, packet.length, 320, undefined, ()=>{
    sock.close();
});