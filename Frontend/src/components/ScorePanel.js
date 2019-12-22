import React from 'react';
import { Card, Accordion } from 'react-bootstrap';
import Table from 'react-bootstrap/Table';

class ScorePanel extends React.Component {
    renderTable() {
        const { scores } = this.props;

        if (scores && scores.collegeCode) {
            return this.renderMajorScores();
        } else if (scores && scores.majorCode) {
            return this.renderCollegeScores();
        }

        return null;
    }

    renderMajorScores() {
        const { collegeCode, collegeName, majors: years } = this.props.scores;
        return (
            <div>
                <h4>Điểm chuẩn của trường {collegeName} (Mã trường: {collegeCode})</h4>
                <Accordion defaultActiveKey="0">
                    {years.map(
                        ({ year, majors }, index) => (
                            <Card key={index}>
                                <Accordion.Toggle as={Card.Header} eventKey={index.toString()} style={{ cursor: 'pointer' }}>
                                    <h5>Năm {year}</h5>
                                </Accordion.Toggle>
                                <Accordion.Collapse eventKey={index.toString()}>
                                    <Card.Body>
                                        <Table bordered hover>
                                            <thead>
                                                <tr>
                                                    <th>STT</th>
                                                    <th>Tên ngành</th>
                                                    <th>Mã ngành</th>
                                                    <th>Tổ hợp môn</th>
                                                    <th>Điểm chuẩn</th>
                                                    <th>Ghi chú</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {majors.map(
                                                    ({ majorName, majorCode, groupCode, score, info }, index) => (
                                                        <tr key={index}>
                                                            <td>{index + 1}</td>
                                                            <td>{majorName}</td>
                                                            <td>{majorCode}</td>
                                                            <td>{groupCode}</td>
                                                            <td>{score}</td>
                                                            <td>{info}</td>
                                                        </tr>
                                                    )
                                                )}
                                            </tbody>
                                        </Table>
                                    </Card.Body>
                                </Accordion.Collapse>
                            </Card>
                        )
                    )}
                </Accordion>
            </div>
        );
    }

    renderCollegeScores() {
        const { majorCode, majorName, colleges: years } = this.props.scores;
        return (
            <div>
                <h4>Điểm chuẩn của ngành/nhóm ngành {majorName} (Mã ngành: {majorCode})</h4>
                <Accordion defaultActiveKey="0">
                    {years.map(
                        ({ year, colleges }, index) => (
                            <Card key={index}>
                                <Accordion.Toggle as={Card.Header} eventKey={index.toString()} style={{ cursor: 'pointer' }}>
                                    <h5>Năm {year}</h5>
                                </Accordion.Toggle>
                                <Accordion.Collapse eventKey={index.toString()}>
                                    <Card.Body>
                                        <Table bordered hover>
                                            <thead>
                                                <tr>
                                                    <th>STT</th>
                                                    <th>Tên trường</th>
                                                    <th>Mã trường</th>
                                                    <th>Tổ hợp môn</th>
                                                    <th>Điểm chuẩn</th>
                                                    <th>Ghi chú</th>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {colleges.map(
                                                    ({ collegeName, collegeCode, groupCode, score, info }, index) => (
                                                        <tr key={index}>
                                                            <td>{index + 1}</td>
                                                            <td>{collegeName}</td>
                                                            <td>{collegeCode}</td>
                                                            <td>{groupCode}</td>
                                                            <td>{score}</td>
                                                            <td>{info}</td>
                                                        </tr>
                                                    )
                                                )}
                                            </tbody>
                                        </Table>
                                    </Card.Body>
                                </Accordion.Collapse>
                            </Card>
                        )
                    )}
                </Accordion>
            </div>
        );
    }

    render() {
        return (
            <div className="score-panel">
                {this.renderTable()}
            </div>
        );
    }
}

export default ScorePanel;
