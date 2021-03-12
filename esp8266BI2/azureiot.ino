void initDevice() {
  deviceClient = IoTHubClient_LL_CreateFromConnectionString(conn, MQTT_Protocol);
}

void sendCallback(IOTHUB_CLIENT_CONFIRMATION_RESULT result, void *userContextCallback) {
  if(IOTHUB_CLIENT_CONFIRMATION_OK == result) {
    Serial.println("Sending message to Azure IoT Hub - succeeded.");
  }
  
  messagePending = false;
}

void sendMessage(char *payload, char *epochTime) {
  IOTHUB_MESSAGE_HANDLE message = IoTHubMessage_CreateFromByteArray((const unsigned char *) payload, strlen(payload));

    MAP_HANDLE properties = IoTHubMessage_Properties(message);
     Map_Add(properties, "type", "distance");
     Map_Add(properties, "SchoolName", "Nackademin");
     Map_Add(properties, "StudentName", "Asma Patel");
     Map_Add(properties, "epochTime", epochTime);

  if(IoTHubClient_LL_SendEventAsync(deviceClient, message, sendCallback, NULL) == IOTHUB_CLIENT_OK) {
    messagePending = true;
  }

  IoTHubMessage_Destroy(message);
}
