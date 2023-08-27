#include "DHT.h"
#include <WiFi.h>
#include <HTTPClient.h>
#include "time.h"
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
#include <avr/power.h> // Required for 16 MHz Adafruit Trinket
#endif
 
#define PIN 5              // Chan Din cua LED1
#define DHTTYPE DHT11       // Loại DHT được sử dụng
#define DHTPIN 25            // Chân dữ liệu của DHT11 kết nối với GPIO25 của ESP32
#define PIN2      21        // Chan Din cua LED2
#define NUMPIXELS 7

hw_timer_t* timer = NULL;
portMUX_TYPE timerMux = portMUX_INITIALIZER_UNLOCKED; 
Adafruit_NeoPixel strip = Adafruit_NeoPixel(8, PIN, NEO_GRB + NEO_KHZ800);
Adafruit_NeoPixel pixels(NUMPIXELS, PIN2, NEO_GRB + NEO_KHZ800);

const char* ssid = "2ag";      // Tên mạng Wifi được chỉ định sẽ kết nối (SSID)
const char* password = "0964246431";  // Password của mạng Wifi được chỉ định sẽ kết nối

int LED1 = 13;
int LED2=15;
int FAN=23;

bool Status[4];
int check=0;
unsigned long time1 = 0;
HTTPClient http;

DHT dht(DHTPIN, DHTTYPE);

  char DataTemp[40];
  char DataHum[40];

void DKLED1()
{
    strip.setPixelColor(0,50,50,0);
    strip.setPixelColor(1,50,0,120);
    strip.setPixelColor(2,255,30,0);
    strip.setPixelColor(3,40,0,100);
    strip.setPixelColor(4,50,0,90);
    strip.setPixelColor(5,50,60,0);
    strip.setPixelColor(6,250,90,30);
    strip.setPixelColor(7,80,130,240);
    strip.show();
}
void DKLED2()
{
  digitalWrite(LED2, HIGH);
  pixels.clear();
  pixels.setBrightness(20);
  pixels.setPixelColor(0, pixels.Color(255, 20, 255));
  pixels.setPixelColor(1, pixels.Color(60, 0, 40));
  pixels.setPixelColor(2, pixels.Color(0, 90, 0));
  pixels.setPixelColor(3, pixels.Color(60, 28, 55));
  pixels.setPixelColor(4, pixels.Color(25, 0, 90));
  pixels.setPixelColor(5, pixels.Color(10, 25, 0));
  pixels.setPixelColor(6, pixels.Color(255, 255, 255));
  pixels.show();
}

void setup()
{
  Serial.begin(115200);
  Serial.printf("Connecting to %s ", ssid);
  WiFi.begin(ssid, password);
  while (WiFi.status() != WL_CONNECTED) {
      delay(500);
      Serial.print(".");
  }
  Serial.println(" CONNECTED");

  dht.begin();
  strip.begin();  
  strip.show();
  pinMode(LED1, OUTPUT);
  pinMode(FAN, OUTPUT);
  pixels.begin();
  pinMode(LED2, OUTPUT);

  
}

void Data()
{
  {
      HTTPClient http;
      float temp = dht.readTemperature();
      float humi = dht.readHumidity();
      if (isnan(temp) || isnan(humi)) 
      {
        Serial.println("Failed to read from DHT sensor!");
        return;
      }
      //    POST temp
      http.begin("https://webapi120220104144706.azurewebsites.net/api/temperature/post");
      http.addHeader("Content-Type", "application/json");
      sprintf(DataTemp,"{\"value\": %f}",temp);
      int httpCode1 = http.POST(DataTemp);
      if (httpCode1 > 0) 
      { //Check for the returning code
 
        Serial.println(httpCode1);
      }
      else 
      {
        Serial.println("Error on HTTP request");
      }
      http.end();

      //  POST humid
      http.begin("https://webapi120220104144706.azurewebsites.net/api/humidity/post");
      http.addHeader("Content-Type", "application/json");
      //sprintf(DataHum,"{\"value\": %f,\"time\": \"%s\"}",humi,timeT);
      sprintf(DataHum,"{\"value\": %f}",humi);
      Serial.println(DataHum);
      int httpCode2 = http.POST(DataHum);
      if (httpCode2 > 0) 
      { //Check for the returning code
 
        Serial.println(httpCode2);
      }
      else 
      {
        Serial.println("Error on HTTP request");
      }
       http.end(); //Free the resources
    }
}
void loop()
{
  
  if ((WiFi.status() == WL_CONNECTED)) 
  { //Check the current connection status
      time1 = millis();
      if (time1>10000)
      {
        Data();
        time1=0;
      }
      http.begin("https://webapi120220104144706.azurewebsites.net/api/remote/currentstates"); 
      int httpCode3 = http.GET();  
      Serial.println(time1); 
      if (httpCode3 > 0) 
      { 
        String payload = http.getString();
        //Serial.println(httpCode3);
        //Serial.println(payload);
        char s=':';
        //clrscr();
        String str=payload;
        int count=0;   // Xu ly du lieu
        while (count<4)
        {
            int vitri=str.indexOf(":");
            str=str.substring(vitri+1,str.length());
            if (str>"true")
            Status[count]=true;
            else
            Status[count]=false;
            Serial.println(str);
            Serial.println(Status[count]);
            count++;
        }
      } 
      http.end();
      Serial.println(time1); 
      
      if (Status[0]==true)
      {
        digitalWrite(LED1, HIGH);
        DKLED1();
      }
      else digitalWrite(LED1, LOW);

      if (Status[1]==true)
      {
        DKLED2();
      }
      else digitalWrite(LED2, LOW);

    
   }
  
}
