import React from 'react';
import Table from 'react-bootstrap/Table';

const PredictPanel = ({ prediction }) => {
    if (!prediction)
        return null;

    if (typeof prediction === 'string')
        return <div>{prediction}</div>;

    const { collegeName, majorName, groupCode, jsonScores: predictions } = prediction;
    return (
        <div className="predict-panel">
            <div>
                <h4>Kết quả dự đoán điểm chuẩn ngành {majorName} của trường {collegeName} - Tổ hợp môn {groupCode}</h4>
                <Table bordered hover>
                    <thead>
                        <tr>
                            <th>STT</th>
                            <th>Năm</th>
                            <th>Điểm chuẩn dự đoán</th>
                            <th>Thuật toán</th>
                        </tr>
                    </thead>
                    <tbody>
                        {predictions.map(
                            ({ year, score, info: algorithm }, index) => (
                                <tr key={index}>
                                    <td>{index + 1}</td>
                                    <td>{year}</td>
                                    <td>{score}</td>
                                    <td>{algorithm}</td>
                                </tr>
                            )
                        )}
                    </tbody>
                </Table>
            </div>
        </div>
    );
};

export default PredictPanel;
