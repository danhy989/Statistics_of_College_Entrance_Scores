export const collegeData = [
    { code: "DCG", name: "Đại học Công Nghệ Thông Tin Gia Định", majorColleges: null },
    { code: "DTC", name: "Đại học Công Nghệ Thông Tin Và Truyền Thông", majorColleges: null },
    { code: "QSC", name: "Đại học Công Nghệ Thông Tin - đại học Quốc Gia Tphcm", majorColleges: null },
    { code: "ANH", name: "Học viện An Ninh Nhân Dân", province_id: 17, province: null, majorColleges: null },
    { code: "ANS", name: "Đại học An Ninh Nhân Dân", province_id: 24, province: null, majorColleges: null },
    { code: "BKA", name: "Đại học Bách Khoa Hà Nội", province_id: 17, province: null, majorColleges: null },
    { code: "BPH", name: "Học viện Biên Phòng - Phía Bắc", province_id: 17, province: null, majorColleges: null },
    { code: "BPS", name: "Học viện Biên Phòng - Phía Nam", province_id: 0, province: null, majorColleges: null },
    { code: "BVH", name: "Học viện Công Nghệ Bưu Chính Viễn Thông", province_id: 17, province: null, majorColleges: null },
    { code: "BVS", name: "Học viện Công Nghệ Bưu Chính Viễn Thông (phía Nam)", province_id: 24, province: null, majorColleges: null },
    { code: "CSH", name: "Học viện Cảnh Sát Nhân Dân", province_id: 17, province: null, majorColleges: null },
    { code: "CSS", name: "Đại học Cảnh Sát Nhân Dân", province_id: 24, province: null, majorColleges: null },
    { code: "CTQ", name: "Đại học Tài Chính Kế Toán", province_id: 42, province: null, majorColleges: null }
];

export const majorData = [
    { code: "714", name: "Khoa học giáo dục và đào tạo giáo viên", majorColleges: null },
    { code: "71401", name: "Khoa học giáo dục", majorColleges: null },
    { code: "7140101", name: "Giáo dục học", majorColleges: null },
    { code: "7140114", name: "Quản lý giáo dục", majorColleges: null },
    { code: "71402", name: "Đào tạo giáo viên", majorColleges: null },
    { code: "7140201", name: "Giáo dục Mầm non", majorColleges: null },
    { code: "7140202", name: "Giáo dục Tiểu học", majorColleges: null },
    { code: "7140203", name: "Giáo dục Đặc biệt", majorColleges: null },
    { code: "7140204", name: "Giáo dục Công dân", majorColleges: null },
    { code: "7140205", name: "Giáo dục Chính trị", majorColleges: null }
];

export const majorScores = {
    took: 1384,
    message: null,
    body: {
        collegeCode: "QSC",
        collegeName: "Đại học Công Nghệ Thông Tin - đại học Quốc Gia Tphcm",
        majors: [
            {
                year: 2018,
                majors: [
                    {
                        majorCode: "7340122",
                        majorName: "Thương mại điện tử",
                        score: 23.9,
                        groupCode: "A00, A01, D01",
                        info: ""
                    },
                    {
                        majorCode: "7340122_CLCA",
                        majorName: "Thương mại điện tử (chất lượng cao)",
                        score: 21.05,
                        groupCode: "A00, A01, D01",
                        info: ""
                    }
                ]
            },
            {
                year: 2019,
                majors: [
                    {
                        majorCode: "7340122",
                        majorName: "Thương mại điện tử",
                        score: 25,
                        groupCode: "A00, A01, D01",
                        info: ""
                    },
                    {
                        majorCode: "7340122_CLCA",
                        majorName: "Thương mại điện tử (chất lượng cao)",
                        score: 24.05,
                        groupCode: "A00, A01, D01",
                        info: ""
                    }
                ]
            }
        ]
    }
};

export const collegeScores = {
    took: 1384,
    message: null,
    body: {
        majorCode: "7340122",
        majorName: "Thương mại điện tử",
        colleges: [
            {
                year: 2018,
                colleges: [
                    {
                        collegeCode: "DVT",
                        collegeName: "Đại học Trà Vinh",
                        score: 15,
                        groupCode: "A00, A01, C01, D01",
                        info: ""
                    },
                    {
                        collegeCode: "DTC",
                        collegeName: "Đại học Công Nghệ Thông Tin Và Truyền Thông",
                        score: 13,
                        groupCode: "A00, C00, C04, D01",
                        info: ""
                    }
                ]
            },
            {
                year: 2019,
                colleges: [
                    {
                        collegeCode: "DVT",
                        collegeName: "Đại học Trà Vinh",
                        score: 18,
                        groupCode: "A00, A01, C01, D01",
                        info: ""
                    },
                    {
                        collegeCode: "DTC",
                        collegeName: "Đại học Công Nghệ Thông Tin Và Truyền Thông",
                        score: 11,
                        groupCode: "A00, C00, C04, D01",
                        info: ""
                    }
                ]
            }
        ]
    }
};

export const majorScoreOverYears = {
    took: 5041,
    message: null,
    body: {
        collegeCode: "QSC",
        collegeName: "Đại học Công Nghệ Thông Tin - đại học Quốc Gia Tphcm",
        majorCode: "7340122",
        majorName: "Thương mại điện tử",
        scores: [
            {
                year: 2017,
                score: 24,
                groupCode: "A00; A01; D01",
                info: ""
            },
            {
                year: 2018,
                score: 21.2,
                groupCode: "A00; A01; D01",
                info: ""
            },
            {
                year: 2019,
                score: 23.9,
                groupCode: "A00; A01; D01",
                info: ""
            }
        ]
    }
};
