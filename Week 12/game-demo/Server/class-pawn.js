const NetworkObject = require("./class-networkobject.js").NetworkObject;

exports.Pawn = class Pawn extends NetworkObject{
    constructor(){
        super();
        this.classID = "PAWN";
    }
    serialize(){
        const b = super.serialize();

        return b;
    }
    deserialize(){
        // TODO: turn object into a byte array
    }
}