import { EquipmentHistory, EquipmentState } from "@/types/types";
import React from "react";

export default function TimelineBar(/*props: {history: EquipmentHistory[]}*/){

  const mockData: EquipmentHistory[] = [
    {fromState: "RED", toState: "YELLOW", timeOfChange: '2025-03-31T00:00:00.4959711', orderId: undefined},
    {fromState: "YELLOW", toState: "GREEN", timeOfChange: '2025-03-31T00:30:24.4959711', orderId: undefined},
    {fromState: "GREEN", toState: "YELLOW", timeOfChange: '2025-03-31T04:50:24.4959711', orderId: undefined},
    {fromState: "YELLOW", toState: "RED", timeOfChange: '2025-03-31T05:50:24.4959711', orderId: undefined},
    {fromState: "RED", toState: "YELLOW", timeOfChange: '2025-03-31T07:50:24.4959711', orderId: undefined},
    {fromState: "YELLOW", toState: "GREEN", timeOfChange: '2025-03-31T08:50:24.4959711', orderId: undefined},
    {fromState: "GREEN", toState: "YELLOW", timeOfChange: '2025-03-31T14:50:24.4959711', orderId: undefined},
    {fromState: "YELLOW", toState: "RED", timeOfChange: '2025-03-31T15:50:24.4959711', orderId: undefined},
    {fromState: "RED", toState: "YELLOW", timeOfChange: '2025-03-31T16:50:24.4959711', orderId: undefined},
    {fromState: "YELLOW", toState: "GREEN", timeOfChange: '2025-03-31T17:50:24.4959711', orderId: undefined}
  ]

  const stateColors: Record<EquipmentState, string> = {
    GREEN: "bg-green-500",
    YELLOW: "bg-yellow-500",
    RED: "bg-red-500",
  };

  const convertTimeToMinutes = (time: string): number => {
    const date = new Date(time);
    return date.getHours() * 60 + date.getMinutes();
  };

  const getTimelineBarWidth = (start: string, end: string): string => {
    const startMinutes = convertTimeToMinutes(start);
    const endMinutes = convertTimeToMinutes(end);
    return ((endMinutes - startMinutes) / 1440) * 100 + "%"; // 24 hours in a day, which is full length, 1440 minutes is 24h
  };

  const segments = mockData.map((entry : EquipmentHistory, index : number) => {
    const prevTransitionTime = index === 0 ? "00:00" : mockData[index - 1].timeOfChange;
    return {
      start: prevTransitionTime,
      end: entry.timeOfChange,
      color: stateColors[entry.fromState] || "bg-gray-400",
    };
  });

  // Get the current running operation up until now
  const [lastTransition] = mockData.slice(-1);
  if(new Date(lastTransition.timeOfChange) < new Date()){
    segments.push({start: lastTransition.timeOfChange, end: new Date().toISOString(), color: stateColors[lastTransition.toState] })
  }

  const timeLabels = Array.from({ length: 9 }, (_, i) => {
    const hour = i * 3;
    return `${hour.toString().padStart(2, "0")}:00`;
  });

  // if(props.history.length === 0) return <p className="text-center text-2xl">No data available</p>

  return (
    <div className="w-full">
      <div className="relative w-full h-8 flex border rounded overflow-hidden">
        {segments.map((seg, index) => (
          <div key={index} className={`h-full ${seg.color}`} style={{ width: getTimelineBarWidth(seg.start, seg.end) }} />
        ))}
      </div>

      <div className="relative w-full flex justify-between text-xs text-gray-700 mt-1">
        {timeLabels.map((label, index) => (
          <div key={index} className="text-center w-1/9">{label}</div>
        ))}
      </div>
    </div>
  );
}