#include <WiFi.h>
#include <Esp32MQTTClient.h>
#include "time.h"
#include <Adafruit_Sensor.h>
#include <DHT.h>
#include <DHT_U.h>
#include <ArduinoJson.h>

char* ssid = "";
char* pass = "";

#define DEVICE_ID "esp32"
#define DHT_PIN 21              // Digital pin connected to the DHT sensor 
#define DHT_TYPE    DHT11     // DHT 11

static DHT dht(DHT_PIN,DHT_TYPE);
char payload[256];
float temperature;
float humidity;
float prevData = 0.0;
float diff = 1.0;


char Timebuff[35];

void setup() {
  
  Serial.begin(115200);
  dht.begin();

  initWifi(); 
  initIotDevice();
  
  char* ntpServer = "pool.ntp.org";
  const long  gmtOffset_sec = 3600;
  const int   daylightOffset_sec = 3600;
 
  //init and get the time
  configTime(gmtOffset_sec, daylightOffset_sec, ntpServer);
  printLocalTime();

  }

void loop() {
  temperature = dht.readTemperature();
  humidity = dht.readHumidity();
 
  
  printLocalTime();
   if(!(std::isnan(temperature)) && !(std::isnan(humidity)))  {
        if (temperature > (prevData + diff) || temperature < (prevData - diff)) {  
                      
              StaticJsonDocument<256> doc;                         //Creating json document 
                    doc["Temperature"]= temperature;
                    doc["Humidity"] = humidity;
                    doc["MsgCreateTime"] = Timebuff;
                    
                    serializeJson(doc,payload);
                    Serial.println(payload);
                    
                    delay(5000);

                     sendMessage(payload); 
                   
                     prevData = temperature; 
           }
    }
 
}
