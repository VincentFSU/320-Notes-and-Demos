const NetworkObject = require("./class-networkobject.js").NetworkObject;

exports.Pawn = class Pawn extends NetworkObject{
    constructor(){
        super();
        this.classID = "PAWN";
    }
    serialize(){
        // TODO: turn object into a byte array
    }
    deserialize(){
        // TODO: turn object into a byte array
    }
}