/*
  Blink

  Turns an LED on for one second, then off for one second, repeatedly.

  Most Arduinos have an on-board LED you can control. On the UNO, MEGA and ZERO
  it is attached to digital pin 13, on MKR1000 on pin 6. LED_BUILTIN is set to
  the correct LED pin independent of which board is used.
  If you want to know what pin the on-board LED is connected to on your Arduino
  model, check the Technical Specs of your board at:
  https://www.arduino.cc/en/Main/Products

  modified 8 May 2014
  by Scott Fitzgerald
  modified 2 Sep 2016
  by Arturo Guadalupi
  modified 8 Sep 2016
  by Colby Newman

  This example code is in the public domain.

  http://www.arduino.cc/en/Tutorial/Blink
*/
//RGB LED pins//
int redLED = 4;
int blueLED = 5;
int greenLED = 6;
int LEDBrightness = 125;
//Joystick input pins//
int joystickX = A6;
int joystickY = A7;
int joystickSelect = 2;
//Touchsensor input pin//
int touchSensor = 3;
//x and y throttle percentages//
float throttleX;
float throttleY;
//tuple of throttles to be printed to Unity serial port//
String throttles = "";
//button debounce vars//
unsigned long debounce = 50;
unsigned long last_press = 0;
//analog smoothing vars//
const int numReadings = 20;
int joystickReadingsX[numReadings] = {0};
int joystickReadingsY[numReadings] = {0};
int readingsSumX = 0;
int readingsSumY = 0;
int readIndex = 0;
//Flags//
bool sendBattery = false;
//Status Var//
float batteryCharge = 0.0;
float batteryFullVolts = 7.4;


// the setup function runs once when you press reset or power the board
void setup() {
  // initialize digital pin LED_BUILTIN as an output.
  pinMode(LED_BUILTIN, OUTPUT);
  pinMode(6,OUTPUT);
  pinMode(5,OUTPUT);
  pinMode(4,OUTPUT);
  pinMode(touchSensor, INPUT);
  pinMode(joystickSelect, INPUT);
  Serial.begin(9600);
}


// the loop function runs over and over again forever
void loop() {
  
  //read and smooth next joystick input to throttle vars
  analogThrottleSmoothing();
  // clear throttles string and add new throttle values seperated by comma
  throttles = "";
  throttles += throttleX;
  throttles += ",";
  throttles += throttleY;

  //check to see if sendBattery flag is set and add battery status value to TX string
  //(consider automating this with a switch statement and a string of flags updated by
  // the serialEvent() function)
  if(sendBattery){
    throttles += ",";
    throttles += (batteryCharge/batteryFullVolts)*100; //convert battery charge volts to percentage
    sendBattery = false;
  }
  
  // print to serial buffer if touch sensor and joystick select are depressed, flush, and delay before next reading
  if(digitalRead(touchSensor)){
    analogWrite(redLED, 0);
    analogWrite(blueLED, LEDBrightness);
    Serial.println(throttles);
    Serial.flush();
    delay(20);
  }
  else if(digitalRead(touchSensor)){
    analogWrite(blueLED, LEDBrightness);
    analogWrite(redLED, 0);
  }
  else{
    analogWrite(redLED, LEDBrightness*1.2);
    analogWrite(blueLED, 0);
  }


/*
 if(digitalRead(3)){
  analogWrite(5, 255);
  //analogWrite(4, 0);
  Serial.print("Hello World");
  Serial.write(27);
  Serial.print("enter.");
  //Serial.flush();
  delay(300);
  }
  else{
    //analogWrite(5, 0);
    //analogWrite(4, 255);
  }

  if(digitalRead(3)){
    analogWrite(5, 255);
    analogWrite(4, 0);
  }
  else{
    analogWrite(5, 0);
    analogWrite(4, 255);
  }

  digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
  delay(1000);                       // wait for a second
  digitalWrite(LED_BUILTIN, LOW);    // turn the LED off by making the voltage LOW
  delay(1000);                       // wait for a second
  */
}

void serialEvent(){
  while(Serial.available()){
    char flag = Serial.read();
    switch(flag){
      case 'B':
        for(int i = 0; i < 3; i++){checkBatteryCharge();} //for a more accurate value measure battery three times
        sendBattery = true;
        break;
      
      default:
        break;
    }
  }
}

void analogThrottleSmoothing(){
  // subtract the last reading:
  readingsSumX -= joystickReadingsX[readIndex];
  readingsSumY -= joystickReadingsY[readIndex];
  // read from the sensor but only assign value if select button is 
  //depressed, write zero otherwise:
  //joystickReadingsX[readIndex] =  digitalRead(joystickSelect)>0?analogRead(joystickX):0;
  //joystickReadingsY[readIndex] = digitalRead(joystickSelect)>0?analogRead(joystickY):0;
  joystickReadingsX[readIndex] =  analogRead(joystickX);
  joystickReadingsY[readIndex] = analogRead(joystickY);
  // add the reading to the total:
  readingsSumX += joystickReadingsX[readIndex];
  readingsSumY += joystickReadingsY[readIndex];
  // advance to the next position in the arrays:
  readIndex++;
  // wrap around if we're at the end of the arrays
  readIndex %= numReadings;
  // calculate averages
  throttleX = readingsSumX/numReadings;
  throttleY = readingsSumY/numReadings;
  //set sensitivity-threshold/offset to register a value
  throttleX = (throttleX > 16)?throttleX:0;
  throttleY = (throttleY > 12)?throttleY:0;
  // center readings range at zero
  throttleX = map(throttleX, 0, 1023, 1000, -1000); // x-axis is flipped
  throttleY = map(throttleY, 0, 1023, -1000, 1000);
  //convert to throttle values to percentages
  throttleX /= 1000;
  throttleY /= 1000;
  
//  Serial.print("x: ");
//  Serial.println(throttleX);
//  Serial.print("y: ");
//  Serial.println(throttleY);
}

//check battery by using 5V vcc rail to measure internal 1.1V reference voltage
void checkBatteryCharge(){
  long result;
  // Read 1.1V reference against AVcc
  ADMUX = _BV(REFS0) | _BV(MUX3) | _BV(MUX2) | _BV(MUX1);
  delay(2); // Wait for Vref to settle
  ADCSRA |= _BV(ADSC); // Convert
  while (bit_is_set(ADCSRA,ADSC));
  result = ADCL;
  result |= ADCH<<8;
  result = 1126400L / result; // Back-calculate AVcc in mV
  batteryCharge = ((float)result)/1000;
}

//Interupt to keep application from restarting
ISR(ADC_vect)
{
}
