#include <AzureIoTHub.h>
#include <AzureIoTProtocol_MQTT.h>
#include <AzureIoTUtility.h>
#include <ESP8266WiFi.h> 

#include <ArduinoJson.h> // downgraded 5.13


char* ssid = "#Telia-CDC9A8";
char* pass = "a6G&3@e=f7(FCa3H";
char *conn = "HostName=iot20-iotasmhub.azure-devices.net;DeviceId=esp8266;SharedAccessKey=bAHzOpMnCLq6ZKV+QX2zC0cWLzjIuFlhU/sqXxs+FIg=";
 //char* conn = "HostName=iot20-iotasmhub.azure-devices.net;DeviceId=esp32;SharedAccessKey=Cp8FArJ8KSvPBK97qXELXxt/aJKIJB9FiHAO1zjK/lM=";

bool messagePending = false;
int interval = 1000 * 15;
unsigned long prevMillis = 0;
time_t epochTime;

int  trigPin = 4;//12 on uno;
int echoPin = 5;//11 on uno;

float pingTravelTime, distance;
float PrevDistance = 0.0;
float diff = 1.0;
float Latitude = 59.35775;
float Longitude = 17.98679;

IOTHUB_CLIENT_LL_HANDLE deviceClient;

void setup() {
  initSerial();
  initWifi();
  initEpochTime();
  pinMode( trigPin,OUTPUT); 
  pinMode(echoPin,INPUT);

  initDevice();
}

void loop() {
  unsigned long currentMillis = millis();
   epochTime = time(NULL);
      Serial.printf("Current Time: %lu. ", epochTime);
      
       digitalWrite(trigPin,LOW); // this creates
       delayMicroseconds(10);     // a pluse of 
       digitalWrite(trigPin,HIGH);// 10 micro seconda
       delayMicroseconds(10);     //
       pingTravelTime = pulseIn(echoPin,HIGH); //pulse command reads the pulse comming in when it sences hig signal

       distance = (pingTravelTime/2)*0.0343; // TO calculate distance in cm
      
    
  if(!messagePending) {
    if((currentMillis - prevMillis) >= interval) {
      prevMillis = currentMillis;

      if ((distance < 40.0 && distance > 2.0) &&(distance > PrevDistance + diff || distance <  PrevDistance - diff)){
          PrevDistance = distance; 
        char payload[256];
        char epochTimeBuf[12];
        
        StaticJsonBuffer<sizeof(payload)> buf;
        JsonObject &root = buf.createObject();
        root["Distance"] =distance;
        root["Latitude"]= Latitude;
        root["Longitude"] = Longitude;
        root.printTo(payload, sizeof(payload));

        sendMessage(payload, itoa(epochTime, epochTimeBuf, 10));      
      }
    }
  }

  IoTHubClient_LL_DoWork(deviceClient);
  delay(10);
}
