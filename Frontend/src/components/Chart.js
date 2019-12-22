import React from 'react';
import { Bar, Line } from 'react-chartjs-2';

export const LineGraph = ({ data, options }) => {
    if (data) {
        return (
            <div>
                <Line data={data} options={options} />
            </div>
        )
    }
    return null;
};

export const BarChart = ({ data, options }) => {
    if (data) {
        return (
            <div>
                <Bar data={data} options={options} />
            </div>
        )
    }
    return null;
};
