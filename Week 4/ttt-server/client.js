

exports.Client = class Client {
    constructor(socket, server){
        this.socket = socket;
        this.server = server;
        this.username = "";

        this.buffer = Buffer.alloc(0);

        this.socket.on("error", (e)=>{this.onError(e)});
        this.socket.on("close", ()=>{this.onClose()});
        this.socket.on("data", (d)=>{this.onData(d)});
    }
    onError(errMsg){
        console.log("ERROR with client: "+errMsg);
    }
    onClose(){
        this.server.onClientDisconnect(this);
    }
    onData(data){
        // add new data to buffer:
        this.buffer = Buffer.concat([this.buffer, data]);
        const lengthOfUsername = this.buffer.readUInt8(4); //gets one byte 4 bytes into the buffer
        // parse buffer for packets:
        if(this.buffer.length < 4) return; // not enough data to process
        
        const packetIdentifier = this.buffer.slice(0, 4).toString();

        switch(packetIdentifier){
            case "JOIN":
                if(this.buffer.length < 5) return; // not enough data to process
                const lengthOfUsername = this.buffer.readUInt8(4);

                if(this.buffer.length < 5 + lengthOfUsername) return; // not enough data to process

                const desiredUsername = this.buffer.slice(5, 5+lengthOfUsername).toString();

                // check username
                console.log("user wants to change name: '"+desiredUsername+"' ");

                 break;
            case "CHAT": break;
            case "PLAY": break;
            default:
                console.log("ERROR: Packet identifier NOT recognized("+packetIdentifier+")");
                this.buffer = Buffer.alloc(0); // wipe the buffer (might cause issues).
                break;
        }
        // process packets (and consume data from buffer)
    }
};