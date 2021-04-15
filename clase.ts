
class IPin {
    private pin: number;
    private value: number;

    constructor(pin: number, value: number) {
        this.pin = pin;
        this.value = value;
    }

    setValue(value: number) {
        this.value = value;
    }
}
class AnalogPin extends IPin{

    constructor(pin: number, value: number) {
        super(pin, value);
    }

}
class DigitalPin extends IPin{
    constructor(pin: number, value: number) {
        super(pin, value);
    }
}

interface ISensor {

    name : string;
    pines: IPin[];

}

class Led implements ISensor{
    name: string;
    pines: IPin[];

    constructor(name: string, pin: number) {
        this.name = name;
        this.pines = [new AnalogPin(pin, 0)];
    }

    
}

let led = new Led("Sala", 12);
led.pines[0].setValue(1);