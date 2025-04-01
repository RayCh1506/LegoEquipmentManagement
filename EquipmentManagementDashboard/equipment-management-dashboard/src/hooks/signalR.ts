import { useEffect, useRef } from "react";
import { HubConnection, HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useQueryClient } from "@tanstack/react-query";

const SIGNALR_URL = "https://localhost:5001/notificationHub";

const useSignalR = (queryKey: string[]) => {
  const queryClient = useQueryClient();
  const connectionRef = useRef<HubConnection | null>(null);

  useEffect(() => {
    if (connectionRef.current) {
      return;
    }

    const connection = new HubConnectionBuilder()
      .withUrl(SIGNALR_URL)
      .configureLogging(LogLevel.Information)
      .withAutomaticReconnect()
      .build();

      connection
      .start()
      .then(() => console.log("Connected to the equipment management system notification hub"))
      .catch(console.error);

      connection.on("StateUpdate", (message) => {
        console.log(`Equipment with id: ${message} has been updated`);
        queryClient.invalidateQueries({ queryKey });
      });

    connectionRef.current = connection;

    return () => {
      console.log("UseEffect signalR cleanup");
      connectionRef.current?.stop();
      connectionRef.current = null;
    };
  }, []);

};

export const useSignalRAll = () => useSignalR(["equipment-all"]);
export const useSignalRWithId = (id: string) => useSignalR([`equipment-${id}`]);