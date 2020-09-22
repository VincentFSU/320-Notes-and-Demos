
// A buffer is a byte array with built in functions
const buff1 = Buffer.from("hello");
const buff2 = Buffer.from([255]);
const buff3 = Buffer.alloc(10);

buff3.writeUInt8(255, 3);
buff3.writeUInt16BE(1024, 5); // Big-Endian notation writes left-to-write

var val = buff3.readUInt8(5);


console.log(val);
console.log(buff3);

