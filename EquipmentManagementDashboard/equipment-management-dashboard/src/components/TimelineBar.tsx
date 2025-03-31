import React from "react";

interface Segment {
  start: string;
  end: string;
  color: string;
}

const TimelineBar: React.FC = () => {
  const segments: Segment[] = [
    { start: "02:00", end: "09:00", color: "bg-green-500" },
    { start: "09:00", end: "10:00", color: "bg-yellow-500" },
    { start: "10:00", end: "10:30", color: "bg-red-500" },
    { start: "10:30", end: "24:00", color: "bg-gray-400" },
  ];

  const getWidth = (start: string, end: string): string => {
    const toMinutes = (time: string): number => {
      const [h, m] = time.split(":").map(Number);
      return h * 60 + (m || 0);
    };
    return ((toMinutes(end) - toMinutes(start)) / 1440) * 100 + "%";
  };

  const timeLabels = Array.from({ length: 9 }, (_, i) => {
    const hour = i * 3;
    return `${hour.toString().padStart(2, "0")}:00`;
  });

  return (
    <div className="w-full">
      <div className="relative w-full h-8 flex border rounded overflow-hidden">
        {segments.map((seg, index) => (
          <div
            key={index}
            className={`h-full ${seg.color}`}
            style={{ width: getWidth(seg.start, seg.end) }}
          />
        ))}
      </div>

      <div className="relative w-full flex justify-between text-xs text-gray-700 mt-1">
        {timeLabels.map((label, index) => (
          <div key={index} className="text-center w-1/9">
            {label}
          </div>
        ))}
      </div>
    </div>
  );
};

export default TimelineBar;