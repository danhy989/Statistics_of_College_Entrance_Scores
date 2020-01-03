import React from 'react';
import { BrowserRouter as Router, Route } from 'react-router-dom';
import { Container, Row } from 'react-bootstrap';

import Header from './Header';
import Sidebar from './Sidebar';
import ContentPanel from './ContentPanel';

import EntranceScore from './EntranceScore';
import { MajorScoreOverYears, CompareScoreBetweenColleges } from './Stats';
import Predict from './Predict';

const App = () => {
    return (
        <Router>
            <div className="page">
                <Header />
                <div className="main">
                    <Container fluid>
                        <Row>
                            <Sidebar>
                                <Sidebar.Item link="/score" title="Tra cứu điểm" />
                                <Sidebar.Item title="Thống kê" eventKey="0">
                                    <Sidebar.Subitem link="/stats/major-score-over-years" title="Điểm chuẩn ngành qua các năm" />
                                    <Sidebar.Subitem link="/stats/compare-score-between-colleges" title="So sánh điểm chuẩn giữa các trường" />
                                </Sidebar.Item>
                                <Sidebar.Item link="/predict" title="Dự đoán" />
                            </Sidebar>

                            <ContentPanel>
                                <Route exact path="/" component={EntranceScore} />
                                <Route path="/score" component={EntranceScore} />
                                <Route path="/stats/major-score-over-years" component={MajorScoreOverYears} />
                                <Route path="/stats/compare-score-between-colleges" component={CompareScoreBetweenColleges} />
                                <Route path="/predict" component={Predict} />
                            </ContentPanel>
                        </Row>
                    </Container>
                </div>
            </div>
        </Router>
    );
}

export default App;
