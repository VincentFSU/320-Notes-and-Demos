exports.NetworkObject = class NetworkObject{
    static _idCount = 0;
    constructor(){
        this.classID = "NWOB";
        this.networkID = ++NetworkObject._idCount;
    }

    serialize(){
        // TODO: turn object into a byte array
    }
    deserialize(){
        // TODO: turn object into a byte array
    }
}