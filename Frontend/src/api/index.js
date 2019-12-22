const host = 'http://35.240.228.120:5000';
const localHost = 'http://localhost:5000';

const basePath = '/api';

const paths = {
    get: {
        college: {
            all: '/College',
            byCode: '/College/',
            byName: '/College/find/',
            byProvince: '/College/province/',
            byGroupCode: '/College/groupCode/'
        },
        major: {
            all: '/Major',
            byCode: '/Major/',
            byName: '/Major/find/',
            byGroupCode: '/Major/groupCode/'
        },
        years: '/MajorCollege/years'
    },
    post: {
        college: {
            majorScores: '/College'
        },
        major: {
            collegeScores: '/Major',
            compare: '/Major/compare'
        },
        predictMajorScore: '/Guess',
        majorScoreOverYears: '/MajorCollege/statistic'
    }
};

function getApiPath(path, param) {
    let apiPath = localHost + basePath + path;
    if (param) {
        apiPath += param;
    }
    return apiPath;
}

function getConfig(httpMethod, data) {
    let configObject = {
        method: httpMethod,
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
        }
    };

    if (data) {
        configObject.body = JSON.stringify(data);
    }

    return configObject;
}

async function fetchData(apiPath, config) {
    let response;

    if (config) {
        response = await fetch(apiPath, config);
    } else {
        response = await fetch(apiPath);
    }

    const data = await response.json();
    return data;
}

export default {
    getAllColleges: async function () {
        const path = getApiPath(paths.get.college.all);
        const data = await fetchData(path);
        return data;
    },

    getCollegeByCode: async function (code) {
        const path = getApiPath(paths.get.college.byCode, code);
        const data = await fetchData(path);
        return data;
    },

    findCollegesByName: async function (name) {
        const path = getApiPath(paths.get.college.byName, name);
        const data = await fetchData(path);
        return data;
    },

    getCollegesByProvince: async function (provinceCode) {
        const path = getApiPath(paths.get.college.byProvince, provinceCode);
        const data = await fetchData(path);
        return data;
    },

    getCollegesByGroupCode: async function (groupCode) {
        const path = getApiPath(paths.get.college.byGroupCode, groupCode);
        const data = await fetchData(path);
        return data;
    },

    getAllMajors: async function () {
        const path = getApiPath(paths.get.major.all);
        const data = await fetchData(path);
        return data;
    },

    getMajorByCode: async function (code) {
        const path = getApiPath(paths.get.major.byCode, code);
        const data = await fetchData(path);
        return data;
    },

    findMajorsByName: async function (name) {
        const path = getApiPath(paths.get.major.byName, name);
        const data = await fetchData(path);
        return data;
    },

    getMajorsByGroupCode: async function (groupCode) {
        const path = getApiPath(paths.get.major.byGroupCode, groupCode);
        const data = await fetchData(path);
        return data;
    },

    getYears: async function () {
        const path = getApiPath(paths.get.years);
        const data = await fetchData(path);
        return data;
    },

    getMajorScoresFromCollege: async function (collegeDTO) {
        const path = getApiPath(paths.post.college.majorScores);
        const config = getConfig('POST', collegeDTO);

        const data = await fetchData(path, config);
        return data;
    },

    getCollegeScoresByMajor: async function (majorDTO) {
        const path = getApiPath(paths.post.major.collegeScores);
        const config = getConfig('POST', majorDTO);

        const data = await fetchData(path, config);
        return data;
    },

    compareMajorScoreBetweenColleges: async function (compareDTO) {
        const path = getApiPath(paths.post.major.compare);
        const config = getConfig('POST', compareDTO);

        const data = await fetchData(path, config);
        return data;
    },

    predictMajorScore: async function (guessDTO) {
        const path = getApiPath(paths.post.predictMajorScore);
        const config = getConfig('POST', guessDTO);

        const data = await fetchData(path, config);
        return data;
    },

    getMajorScoreOverYears: async function (majorCollegeDTO) {
        const path = getApiPath(paths.post.majorScoreOverYears);
        const config = getConfig('POST', majorCollegeDTO);

        const data = await fetchData(path, config);
        return data;
    }
};
