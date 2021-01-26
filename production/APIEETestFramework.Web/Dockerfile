FROM mcr.microsoft.com/dotnet/core/sdk:2.2-alpine3.10
ARG dotnet_cli_home_arg=/tmp/
ENV DOTNET_CLI_HOME=$dotnet_cli_home_arg
RUN echo 'http://dl-3.alpinelinux.org/alpine/v3.12/main'
RUN apk upgrade
COPY . /repo
WORKDIR /repo
RUN  chgrp -Rf root "/repo" && \
     chmod -Rf g+w "/repo" && \
     addgroup --system appuser && \
     adduser --system appuser --ingroup appuser && \
     chown appuser /repo
RUN mkdir TestResults
RUN  chgrp -Rf root "/repo/TestResults" && \
     chmod -Rf g+w "/repo/TestResults" && \
     chown appuser /repo/TestResults
RUN  chgrp -Rf root "/repo/Specflow_TestExecution.sh" && \
     chmod -Rf g+w "/repo/Specflow_TestExecution.sh" && \
     chown appuser /repo/Specflow_TestExecution.sh
RUN ["chmod", "+x", "/repo/Specflow_TestExecution.sh"]
RUN  chgrp -Rf root "/repo/Trxer.xslt" && \
     chmod -Rf g+w "/repo/Trxer.xslt" && \
     chown appuser /repo/Trxer.xslt
RUN  chgrp -Rf root "/repo/Trxer.css" && \
     chmod -Rf g+w "/repo/Trxer.css" && \
     chown appuser /repo/Trxer.css
RUN  chgrp -Rf root "/repo/TrxerTable.css" && \
     chmod -Rf g+w "/repo/TrxerTable.css" && \
     chown appuser /repo/TrxerTable.css
RUN  chgrp -Rf root "/repo/functions.js" && \
     chmod -Rf g+w "/repo/functions.js" && \
     chown appuser /repo/functions.js
USER appuser
CMD ["sh", "-c", "./Specflow_TestExecution.sh"]